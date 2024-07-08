using Photography_Tools.Models.Interfaces;

namespace Photography_Tools.Models.UserInput;

public abstract class UserInput : IValidatable
{
    public abstract bool Validate();
}
