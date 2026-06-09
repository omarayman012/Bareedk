using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using BaridikExpress.Application.Interfaces;

namespace BaridikExpress.Infrastructure.Services;

public class AutoPermissionScanner(IEnumerable<Assembly> assemblies) : IAutoPermissionScanner
{
    public IEnumerable<string> ScanPermissions()
    {
        return assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(ControllerBase)) && !t.IsAbstract)
            .SelectMany(controller =>
            {
                var controllerName = controller.Name.Replace("Controller", "");

                return controller
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(m => !m.IsSpecialName)
                    .Select(action => $"{controllerName}.{action.Name}");
            })
            .Distinct();
    }
}