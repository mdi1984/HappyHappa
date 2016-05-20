using HappyHappa.REST.DAL.Model;
using System.Threading.Tasks;

namespace HappyHappa.REST.DAL
{
  public interface IDAL
  {
    Task<Model.Item> PutItem(BoughtItem item);
    Task<Model.Item> TakeItem(BoughtItem item);

    Task<string> CreateFridge(Device device);

    Task<User> GetUser();
    Task<User> SetUser(User user);
  }
}
