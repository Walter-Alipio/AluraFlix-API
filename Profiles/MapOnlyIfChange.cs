
using AutoMapper;

public static class MapOnlyIfChange
{
  public static IMappingExpression<TSource, TDestination> MapOnlyIfChanged<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map)
  {
    map.ForAllMembers(source =>
        {
          source.Condition((sourceObject, destObject, sourceProperty, destProperty) =>
          {
            if (sourceProperty == null)
              return (destProperty == null);
            return !sourceProperty.Equals(destProperty);
          });
        });

    return map;
  }
}
//https://stackoverflow.com/questions/5781099/map-only-changed-properties