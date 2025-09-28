namespace KedemMarket.Factories.Vendors;

public class VendorApiModelFactory : IVendorApiModelFactory
{
    private readonly IVendorService _vendorService;
    private readonly IPictureService _pictureService;
    private readonly IAddressService _addressService;
<<<<<<< HEAD
    private readonly MediaConvertor _mediaPreperar;
=======
    private readonly IMediaManager _mediaPreperar;
>>>>>>> dev/get-vendors-sales
    private readonly IAttributeParser<AddressAttribute, AddressAttributeValue> _addressAttributeParser;
    private readonly IAttributeService<AddressAttribute, AddressAttributeValue> _addressAttributeService;
    private readonly IDirectoryFactory _directoryFactory;
    private readonly IUrlRecordService _urlRecordService;
    private readonly IStoreContext _storeContext;

    public VendorApiModelFactory(
        IPictureService pictureService,
        IAddressService addressService,
<<<<<<< HEAD
        MediaConvertor mediaPreperar,
=======
        IMediaManager mediaPreperar,
>>>>>>> dev/get-vendors-sales
        IAttributeParser<AddressAttribute, AddressAttributeValue> addressAttributeParser,
        IAttributeService<AddressAttribute, AddressAttributeValue> addressAttributeService,
        IDirectoryFactory directoryFactory,
        IVendorService vendorService,
        IUrlRecordService urlRecordService,
        IStoreContext storeContext)
    {
        _vendorService = vendorService;
        _pictureService = pictureService;
        _addressService = addressService;
        _mediaPreperar = mediaPreperar;
        _addressAttributeParser = addressAttributeParser;
        _addressAttributeService = addressAttributeService;
        _directoryFactory = directoryFactory;
        _urlRecordService = urlRecordService;
        _storeContext = storeContext;
    }

    public async Task<IList<VendorApiModel>> GetAllVendorsAsync()
    {
        var vendors = await _vendorService.GetAllVendorsAsync();
        var res = new List<VendorApiModel>();
        foreach (var v in vendors)
            res.Add(await PrepareVendorApiModelAsync(v));

        return res;
    }

    public async Task<VendorApiModel> PrepareVendorApiModelAsync(Vendor vendor)
    {
        var address = await _addressService.GetAddressByIdAsync(vendor.AddressId);
        var picture = await _pictureService.GetPictureByIdAsync(vendor.PictureId);
        var image = picture != default ? await _mediaPreperar.ToGalleryItemModel(picture, 0) : default;
        var store = await _storeContext.GetCurrentStoreAsync();
        var slug = await _urlRecordService.GetSeNameAsync(vendor, languageId: store.DefaultLanguageId);

        var contactInfo = await ToContactInfo(vendor, address);
        return new()
        {
            Id = vendor.Id,
            Name = vendor.Name,
            Description = vendor.Description,
            ContactInfo = contactInfo,
            DisplayOrder = vendor.DisplayOrder,
            MetaKeywords = vendor.MetaKeywords,
            MetaDescription = vendor.MetaDescription,
            MetaTitle = vendor.MetaTitle,
            Image = image,
            Slug = slug,
        };
        //PictureId,
    }

    private async Task<ContactInfoModel?> ToContactInfo(Vendor vendor, Address? address)
    {
        var attribute = (await _addressAttributeService.GetAllAttributesAsync())
            .FirstOrDefault(a => a.Name.Equals("comment", StringComparison.CurrentCultureIgnoreCase));

        var comment = default(string);

        if (address != null && attribute != default && address.CustomAttributes != default)
        {
            var enteredText = _addressAttributeParser.ParseValues(address?.CustomAttributes, attribute.Id);
            if (enteredText.Any())
                comment = enteredText[0];
        }

        var addressModel = default(AddressApiModel);
        if (address.City != null)
        {
            var street = buildTrimedString(address?.Address1, address?.Address2);
            if (street.HasNoValue())
                street = null;
            addressModel = new AddressApiModel
            {
                City = address.City,
                PostalCode = address.ZipPostalCode,
                Street = street,
            };
        }

        return new()
        {
            Address = addressModel,
            Comment = comment,
            Email = address?.Email ?? vendor.Email,
            Fullname = buildTrimedString(address?.FirstName ?? vendor.Name, address?.LastName),
            Phone = _directoryFactory.ProcessPhoneNumber(address?.PhoneNumber),
        };

        static string buildTrimedString(string str1, string str2) =>
            $"{str1 ?? ""} {str2 ?? ""}".Trim();
    }
}
