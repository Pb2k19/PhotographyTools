namespace Photography_Tools.Models;

public record ServiceResponse<T>(T Data, bool IsSuccess = false, int Code = -1, string? Message = null);