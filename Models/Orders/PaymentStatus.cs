using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Models.Orders
{
    public enum PaymentStatus
    {
        [EnumMember(Value= "Pending")]
        Pending,
        [EnumMember(Value= "Payment Recevied")]
        PaymentRecevied,
        [EnumMember(Value= "Payment Failed")]
        PaymentFailed,
        [EnumMember(Value= "Payment Cancelled")]  
        PaymentCancelled,
        [EnumMember(Value= "Payment On Hold")]
        PaymentOnHold,
        [EnumMember(Value = "Processing")]
        Processing,
        [EnumMember(Value = "Submitted")]
        Submitted,
    }
}