namespace BaridikExpress.Application.Interfaces;

public interface IAutoPermissionScanner
{
    IEnumerable<string> ScanPermissions();
}