using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bogus;
using FsCheck.Xunit;
using TddWorkshop.Domain.InstantCredit;
using TddWorkshop.Domain.Tests.Arbitraries;
using TddWorkshop.Domain.Tests.Extensions;
using Xunit;
using static TddWorkshop.Domain.InstantCredit.CreditGoal;
using static TddWorkshop.Domain.InstantCredit.Employment;

namespace TddWorkshop.Domain.Tests;

public class CreditCalculatorTests
{
    [Theory, ClassData(typeof(CreditCalculatorTestData))]
    public async Task Calculate_IsApproved_PointsCalculatedCorrectly(
    CalculateCreditRequest request, bool hasCriminalRecord, int points)
    {
        var creditCalculator = new CreditCalculator(new CriminalRecordCheckerMock(hasCriminalRecord));
        var result = await creditCalculator.CalculateAsync(request, hasCriminalRecord);

        Assert.Equal(points, result.Points);
    }
    
    [Theory, AutoData]
    public async Task Calculate_AutoData_NoException(
        CalculateCreditRequest request, bool hasCriminalRecord)
    {
        var creditCalculator = new CreditCalculator(new CriminalRecordCheckerMock(hasCriminalRecord));
        var result = await creditCalculator.CalculateAsync(request, hasCriminalRecord);
    }
}

public class CreditCalculatorTestData : IEnumerable<object[]>
{
    public static readonly CalculateCreditRequest Maximum =
        CreateRequest(30, ConsumerCredit, 1_000_001, Deposit.RealEstate, Employee, false);
    
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] // 100 points - 12,5%
            { Maximum, false, 100 };

        yield return new object[] // 85 points - 26%
            { CreateRequest(30, ConsumerCredit, 1_000_001, Deposit.RealEstate, Employee, false), true, 85 };

        yield return new object[] // 16 points
            { CreateRequest(21, RealEstate, 5_000_001, Deposit.None, Unemployed, true), true, 16 };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static CalculateCreditRequest CreateRequest(int age, CreditGoal goal, decimal sum,
        Deposit deposit, Employment employment, bool hasOtherCredits)
    {
        // var faker = new Faker();
        return new CalculateCreditRequest(
            new PersonalInfo(age, "", ""),
            new CreditInfo(goal, sum, deposit, employment, hasOtherCredits),
            new PassportInfo("1234", "123456", DateTime.Now, "")
        );
    }
}