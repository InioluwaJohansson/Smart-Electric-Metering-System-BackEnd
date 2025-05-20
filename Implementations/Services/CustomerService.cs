using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Entities.Identity;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using static System.Net.Mime.MediaTypeNames;

namespace Smart_Electric_Metering_System_BackEnd.Implementations.Services;
public class CustomerService : ICustomerService
{
    IUserRepo _userRepo;
    ICustomerRepo _customerRepo;
    IMeterRepo _meterRepo;
    public CustomerService(IUserRepo userRepo,ICustomerRepo customerRepo, IMeterRepo meterRepo)
    {
        _customerRepo = customerRepo;
        _userRepo = userRepo;
        _meterRepo = meterRepo;
    }
    public async Task<BaseResponse> CreateCustomer(CreateCustomerDto createCustomerDto)
    {
        var customer = await _userRepo.Get(x => x.Email == createCustomerDto.createUserDto.Email);
        if(customer != null)
        {
            return new BaseResponse{
                Status = false,
                Message = $"{createCustomerDto.createUserDto.Email} has been used!"
            };
        }
        var user = new User{
            FirstName = createCustomerDto.createUserDto.FirstName,
            LastName = createCustomerDto.createUserDto.LastName,
            Email = createCustomerDto.createUserDto.Email,
            UserName = "",
            PhoneNumber = "",
            PictureUrl = "",
            Password = BCrypt.Net.BCrypt.HashPassword(createCustomerDto.createUserDto.Password),
        };
        var userrole = await _userRepo.Create(user);
        var userRole = new UserRole {
            UserId = userrole.Id,
            Role = Role.Customer
        };
        userrole.UserRole = userRole;
        await _userRepo.Update(userrole);
        var NewCustomer =  new Customer {
            CustomerId = $"CUSTOMER{Guid.NewGuid().ToString().Substring(0,8).Replace("-", "").ToUpper()}",
            UserId = userrole.Id,
            Notification = new Notification{
                PeakUsageAlerts = false,
                UsageThresholdAlerts = false,
                UsageAlerts = false,
                BillingNotifications = false,
                PushNotifications = false,
            },
        };
        await _customerRepo.Create(NewCustomer);
        return new BaseResponse{
            Status = true,
            Message = "Account Created!"
        };
    }
    public async Task<BaseResponse> UpdateCustomer(UpdateCustomerDto updateCustomerDto)
    {
        var customer = await _customerRepo.GetById(updateCustomerDto.Id);
        if(customer != null)
        {
            // var folderPath = Path.Combine(Directory.GetCurrentDirectory() + "..\\..\\Images\\");
            // if (!System.IO.Directory.Exists(folderPath))
            // {
            //     Directory.CreateDirectory(folderPath);
            // }
            // var imagePath = "";
            // if (updateCustomerDto.Picture != null)
            // {
            //     /*var fileName = Path.GetFileNameWithoutExtension(updateCustomerDto.Picture.FileName);
            //     var filePath = Path.Combine(folderPath, updateCustomerDto.Picture.FileName);
            //     var extension = Path.GetExtension(updateCustomerDto.Picture.FileName);
            //     if (!System.IO.Directory.Exists(filePath))
            //     {
            //         using (var stream = new FileStream(filePath, FileMode.Create))
            //         {
            //             await updateCustomerDto.Picture.CopyToAsync(stream);
            //         }
            //         imagePath = fileName;
            //     }*/
            //     using(var memoryStream = new MemoryStream()){
            //         updateCustomerDto.Picture.CopyTo(memoryStream);
            //         imagePath = Convert.ToBase64String(memoryStream.ToArray());
            //     }
            // }
            customer.User.UserName = updateCustomerDto.UserName ?? customer.User.UserName;
            customer.User.Email = updateCustomerDto.Email ?? customer.User.Email;
            customer.User.FirstName = updateCustomerDto.FirstName ?? customer.User.FirstName;
            customer.User.LastName = updateCustomerDto.LastName ?? customer.User.LastName;
            customer.User.PhoneNumber = updateCustomerDto.PhoneNumber ?? customer.User.PhoneNumber;
            customer.User.PictureUrl = updateCustomerDto.Picture ?? customer.User.PictureUrl;
            customer.Notification.PeakUsageAlerts = updateCustomerDto.PeakUsageAlerts;
            customer.Notification.UsageThresholdAlerts = updateCustomerDto.UsageThresholdAlerts;
            customer.Notification.UsageAlerts = updateCustomerDto.UsageAlerts;
            customer.Notification.BillingNotifications = updateCustomerDto.BillingNotifications;
            customer.Notification.PushNotifications = updateCustomerDto.PushNotifications;
            customer.LastModifiedOn = DateTime.Now;
            await _customerRepo.Update(customer);
            return new BaseResponse{
                Status = true,
                Message = "Account Updated!"
            };
        }
        return new BaseResponse{
            Status = false,
            Message = "Unable to find account!"
        };
    }
    public async Task<CustomerResponse> GetCustomerById(int id){
        var customer = await _customerRepo.GetById(id);
        if (customer == null)
        {
            return new CustomerResponse
            {
            Status = false,
            Message = "Customer not found!"
            };
        }
        return new CustomerResponse ()
        {
            Status = true,
            Message = "Customer retrieved successfully!",
            Data = await GetCustomerDetails(customer),
        };
    }
    public async Task<CustomersResponse> GetAllCustomers(){
        var consumer = await _customerRepo.GetCustomers();
        if(consumer != null){
            List<GetCustomerDto> customers = new List<GetCustomerDto>();
            foreach (var item in consumer)
            {
                var customer = await GetCustomerDetails(item);
                customers.Add(customer);
            }
            return new CustomersResponse{
                Data = customers,
                Status =  true,
                Message = "Consumers Data Retrieved!"
            };
        }
        return new CustomersResponse{
            Data = null,
            Status =  false,
            Message = "Unable to Retrieve Consumers Data!"
        };
    }
    public async Task<GetCustomerDto> GetCustomerDetails(Customer customer)
    {
        var meters = await _meterRepo.GetByExpression(x => x.UserId == customer.User.Id);
        return new GetCustomerDto ()
        {
            Id = customer.Id,
            CustomerId = customer.CustomerId,
            FirstName = customer.User.FirstName,
            LastName = customer.User.LastName,
            UserName = customer.User.UserName,
            Email = customer.User.Email,
            PhoneNumber = customer.User.PhoneNumber,
            PictureUrl = customer.User.PictureUrl,
            CreatedOn = customer.CreatedOn,
            getMeterDto = meters.Select(x => new GetMeterDto{
                Id = x.Id,
                MeterId = x.MeterId,
                CustomerName = customer.User.FirstName + " " + customer.User.LastName,
                TotalUnits = x.TotalUnits,
                ConsumedUnits = x.ConsumedUnits,
            }).ToList(),
            getNotificationDto = new GetNotificationDto{
                PeakUsageAlerts = customer.Notification.PeakUsageAlerts,
                UsageThresholdAlerts = customer.Notification.UsageThresholdAlerts,
                UsageAlerts = customer.Notification.UsageAlerts,
                BillingNotifications = customer.Notification.BillingNotifications,
                PushNotifications = customer.Notification.PushNotifications,
            },
        };
    }
}
