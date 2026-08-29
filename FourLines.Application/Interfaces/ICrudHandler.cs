namespace FourLines.Application.Interfaces;

public interface ICrudHandler<TEntity, TCreateDto, TUpdateDto, TDeleteDto> where TEntity : BaseEntity
{
    Task<Result<TEntity>> Create(TCreateDto createDto);
    Task<Result<TEntity>> Update(TUpdateDto updateDto);
    Task<Result<bool>> Delete(TDeleteDto deleteDto);
}
