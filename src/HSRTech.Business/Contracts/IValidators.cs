using HSRTech.Business.UseCases.Validators;

namespace HSRTech.Business.Contracts
{
    public interface IValidators
    {
        ResultValidation Validate(object entity);
    }
}
