namespace TddWorkshop.Domain.InstantCredit;

public interface ICriminalRecordChecker
{
    public Task<bool> HasCriminalRecord(PersonalInfo record, CancellationToken cancellationToken);
}