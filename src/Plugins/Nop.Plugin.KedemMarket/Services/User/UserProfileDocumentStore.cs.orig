using KedemMarket.Documents;
<<<<<<< HEAD
using KedemMarket.Services.Documents;
=======
>>>>>>> dev/get-vendors-sales

namespace KedemMarket.Services.User;

public class UserProfileDocumentStore : FirebaseDocumentStore<UserProfileDocument>, IUserProfileDocumentStore
{
    public UserProfileDocumentStore(IConfiguration configuration) : base(configuration, "user-profiles")
    {
    }

    public async Task<UserProfileDocument> GetByUserId(string uid)
    {
        var collection = GetFireStoreCollection();

        var q = await collection.WhereEqualTo("userId", uid).GetSnapshotAsync();
        var res = q.Documents.Select(d => d.ConvertTo<UserProfileDocument>());
        return res?.FirstOrDefault();
    }
}
