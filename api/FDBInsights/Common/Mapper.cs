using FastEndpoints;
using System.Reflection;

namespace FDBInsights.Common;

// Simple implementation of IMapper for FastEndpoints
public class AppMapper : IMapper
{
    public TDestination Map<TDestination>(object source) where TDestination : new()
    {
        // This is a very simple implementation
        // In a real application, you'd use a proper mapping library like AutoMapper
        var destination = new TDestination();
        
        // Get all properties from source
        var sourceProps = source.GetType().GetProperties();
        var destProps = typeof(TDestination).GetProperties();
        
        foreach (var sourceProp in sourceProps)
        {
            var destProp = destProps.FirstOrDefault(p => 
                p.Name == sourceProp.Name && 
                p.CanWrite && 
                p.PropertyType.IsAssignableFrom(sourceProp.PropertyType));
                
            if (destProp != null)
            {
                var value = sourceProp.GetValue(source);
                destProp.SetValue(destination, value);
            }
        }
        
        return destination;
    }
}