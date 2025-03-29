using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Entities.Identity;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;

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
        var customer = await _userRepo.Get(x => x.UserName == createCustomerDto.createUserDto.UserName);
        if(customer != null)
        {
            return new BaseResponse{
                Status = false,
                Message = "Username Taken!"
            };
        }
        var user = new User{
            FirstName = createCustomerDto.createUserDto.FirstName,
            LastName = createCustomerDto.createUserDto.LastName,
            UserName = createCustomerDto.createUserDto.UserName,
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
            CustomerId = $"CUSTOMER{Guid.NewGuid().ToString().Substring(0,5).ToLower()}",
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
            var folderPath = Path.Combine(Directory.GetCurrentDirectory() + "..\\..\\Images\\");
            if (!System.IO.Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var imagePath = "";
            if (updateCustomerDto.Picture != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(updateCustomerDto.Picture.FileName);
                var filePath = Path.Combine(folderPath, updateCustomerDto.Picture.FileName);
                var extension = Path.GetExtension(updateCustomerDto.Picture.FileName);
                if (!System.IO.Directory.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await updateCustomerDto.Picture.CopyToAsync(stream);
                    }
                    imagePath = fileName;
                }
            }
            customer.User.FirstName = updateCustomerDto.FirstName ?? customer.User.FirstName;
            customer.User.LastName = updateCustomerDto.LastName ?? customer.User.LastName;
            customer.User.PictureUrl = imagePath ?? customer.User.PictureUrl;
            customer.Notification.PeakUsageAlerts = updateCustomerDto.updateNotificationDto.PeakUsageAlerts;
            customer.Notification.UsageThresholdAlerts = updateCustomerDto.updateNotificationDto.UsageThresholdAlerts;
            customer.Notification.UsageAlerts = updateCustomerDto.updateNotificationDto.UsageAlerts;
            customer.Notification.BillingNotifications = updateCustomerDto.updateNotificationDto.BillingNotifications;
            customer.Notification.PushNotifications = updateCustomerDto.updateNotificationDto.PushNotifications;
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
        var customer = await _customerRepo.Get(c => c.Id == id);
        if (customer == null)
        {
            return new CustomerResponse
            {
            Status = false,
            Message = "Customer not found!"
            };
        }
        var user = await _userRepo.Get(u => u.Id == customer.UserId);
        if (user == null)
        {
            return new CustomerResponse
            {
            Status = false,
            Message = "User not found!"
            };
        }
        return new CustomerResponse
        {
            Status = true,
            Message = "Customer retrieved successfully!",
            Data = await GetCustomerDetails(customer),
        };
    }
    public async Task<CustomersResponse> GetAllCustomers(){
        var consumer = await _customerRepo.GetByExpression(x => x.IsDeleted == false);
        if(consumer != null){
            return new CustomersResponse{
                Data = (ICollection<GetCustomerDto>)consumer.Select(x => GetCustomerDetails(x)).ToList(),
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
        var user = await _userRepo.Get(u => u.Id == customer.UserId);
        var meters = await _meterRepo.GetByExpression(x =>x.UserId == user.Id) ?? null;
        if (user == null)
        {
            return null;
        }
        return new GetCustomerDto
        {
            Id = customer.Id,
            CustomerId = customer.CustomerId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            getMeterDto = meters.Select(x => new GetMeterDto{
                Id = x.Id,
                MeterId = x.MeterId
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
