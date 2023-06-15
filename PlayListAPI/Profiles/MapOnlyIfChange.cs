
using AutoMapper;

namespace PlayListAPI.Profiles;

public static class MapOnlyIfChange
{
  public static IMappingExpression<TSource, TDestination> MapOnlyIfChanged<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map)
  {
    map.ForAllMembers(source =>
        {
          source.Condition((sourceObject, destObject, sourceProperty, destProperty) =>
          {
            if (sourceProperty == null)
            {
              // Se a propriedade de origem for nula, ignorar a propriedade de destino
              return false;
            }
            // Se a propriedade de destino for nulável, definir como null, caso contrário, definir um valor padrão seguro
            return !sourceProperty.Equals(destProperty)
              && (Nullable.GetUnderlyingType(destProperty.GetType()) != null ? true : destProperty.Equals(default));
          });
        });

    return map;
  }
}
//https://stackoverflow.com/questions/5781099/map-only-changed-properties