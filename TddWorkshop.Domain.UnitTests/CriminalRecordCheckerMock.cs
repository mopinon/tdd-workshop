using System.Threading;
using System.Threading.Tasks;
using TddWorkshop.Domain.InstantCredit;

namespace TddWorkshop.Domain.Tests;

public class CriminalRecordCheckerMock : ICriminalRecordChecker
{
    private readonly bool _hasCriminal;

    public CriminalRecordCheckerMock(bool hasCriminal)
    {
        _hasCriminal = hasCriminal;
    }

    public async Task<bool> HasCriminalRecord(PersonalInfo record, CancellationToken cancellationToken)
    {
        return _hasCriminal;
    }
}