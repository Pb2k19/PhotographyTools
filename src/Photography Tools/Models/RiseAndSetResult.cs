namespace Photography_Tools.Models;

public readonly record struct RiseAndSetResult(DateTime? Rise, DateTime? Set, bool AlwaysUp, bool AlwaysDown)
{
    public RiseAndSetResult(DateTime rise, DateTime set) : this(rise, set, false, false) { }

    public RiseAndSetResult(DateTime? rise, DateTime? set, double ye) :
        this(rise, set, !rise.HasValue && !set.HasValue && ye > 0, !rise.HasValue && !set.HasValue && ye <= 0)
    { }
}