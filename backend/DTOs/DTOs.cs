namespace EMSBackend.DTOs
{
    // Auth DTOs
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FullName { get; set; }
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string TokenType { get; set; } = "Bearer";
        public UserDTO User { get; set; } = null!;
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuperuser { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Power Station DTOs
    public class PowerStationRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Capacity { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
    }

    public class PowerStationResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Capacity { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // Device DTOs
    public class DeviceRequest
    {
        public int PowerStationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public string? Specifications { get; set; }
        public DateTime? InstallationDate { get; set; }
        public DateTime? WarrantyExpiry { get; set; }
    }

    public class DeviceResponse
    {
        public int Id { get; set; }
        public int PowerStationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public string? Specifications { get; set; }
        public DateTime? InstallationDate { get; set; }
        public DateTime? WarrantyExpiry { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // Device Monitoring DTOs
    public class DeviceMonitoringRequest
    {
        public int DeviceId { get; set; }
        public double? BatteryLevel { get; set; }
        public double? Temperature { get; set; }
        public double? Voltage { get; set; }
        public double? Current { get; set; }
        public double? PowerOutput { get; set; }
        public double? EnergyGenerated { get; set; }
        public double? EnergyConsumed { get; set; }
        public string? Status { get; set; }
        public string? ErrorCode { get; set; }
    }

    public class DeviceMonitoringResponse
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public double? BatteryLevel { get; set; }
        public double? Temperature { get; set; }
        public double? Voltage { get; set; }
        public double? Current { get; set; }
        public double? PowerOutput { get; set; }
        public double? EnergyGenerated { get; set; }
        public double? EnergyConsumed { get; set; }
        public string? Status { get; set; }
        public string? ErrorCode { get; set; }
        public DateTime RecordedAt { get; set; }
    }

    // Event DTOs
    public class EventRequest
    {
        public int? DeviceId { get; set; }
        public int? PowerStationId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string? Severity { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class EventResponse
    {
        public int Id { get; set; }
        public int? DeviceId { get; set; }
        public int? PowerStationId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string? Severity { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Resolved { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string? ResolvedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Factory Test DTOs
    public class FactoryTestRequest
    {
        public int DeviceId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public string? TestType { get; set; }
        public string? Status { get; set; }
        public string? Result { get; set; }
        public string? TestedBy { get; set; }
        public string? Notes { get; set; }
    }

    public class FactoryTestResponse
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public string? TestType { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Result { get; set; }
        public string? TestedBy { get; set; }
        public DateTime? TestedAt { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // Construction Process DTOs
    public class ConstructionProcessRequest
    {
        public int PowerStationId { get; set; }
        public string Phase { get; set; } = string.Empty;
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ExpectedCompletion { get; set; }
        public string? ResponsiblePerson { get; set; }
        public string? Description { get; set; }
        public string? Documents { get; set; }
    }

    public class ConstructionProcessResponse
    {
        public int Id { get; set; }
        public int PowerStationId { get; set; }
        public string Phase { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ExpectedCompletion { get; set; }
        public string? ResponsiblePerson { get; set; }
        public string? Description { get; set; }
        public string? Documents { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // Statistics DTOs
    public class StatisticsRequest
    {
        public int? PowerStationId { get; set; }
        public string StatisticType { get; set; } = string.Empty;
        public string? Period { get; set; }
        public DateTime? PeriodStart { get; set; }
        public DateTime? PeriodEnd { get; set; }
        public double? Value { get; set; }
        public string? Unit { get; set; }
        public string? Data { get; set; }
    }

    public class StatisticsResponse
    {
        public int Id { get; set; }
        public int? PowerStationId { get; set; }
        public string StatisticType { get; set; } = string.Empty;
        public string? Period { get; set; }
        public DateTime? PeriodStart { get; set; }
        public DateTime? PeriodEnd { get; set; }
        public double? Value { get; set; }
        public string? Unit { get; set; }
        public string? Data { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class StatisticsSummaryResponse
    {
        public double EnergyGeneratedTotal { get; set; }
        public double EnergyConsumedTotal { get; set; }
        public int TotalEvents { get; set; }
        public int UnresolvedEvents { get; set; }
        public string Period { get; set; } = "all_time";
    }
}
