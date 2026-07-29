namespace KinematicsSimulator.Application.Common.Models;

public record PagedList<T>(
    IReadOnlyCollection<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);
