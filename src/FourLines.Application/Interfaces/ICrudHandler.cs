namespace FourLines.Application.Interfaces;

public interface ICrudHandler<TEntity, TCreateDto, TUpdateDto, TDeleteDto>
    where TEntity : BaseEntity
{
    Task<Result<TEntity>> Create(
        TCreateDto createDto,
        CancellationToken cancellationToken = default
    );
    Task<Result<TEntity>> Update(
        TUpdateDto updateDto,
        CancellationToken cancellationToken = default
    );
    Task<Result<bool>> Delete(TDeleteDto deleteDto, CancellationToken cancellationToken = default);
}
