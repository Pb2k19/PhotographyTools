using Photography_Tools.Models.Interfaces;

namespace Photography_Tools.Models;

public abstract class UserInput : IValidatable
{
    public abstract bool Validate();
}
