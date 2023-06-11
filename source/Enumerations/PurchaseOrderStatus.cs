using FishbowlInventory.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Enumerations
{
    /// <summary>
    /// The order status.
    /// </summary>
    /// <remarks>
    /// 'All' | 'All Open' | 'Bid Request' | 'Pending Approval' | 'Issued' | 'Picking' | 'Partial' | 'Picked' | 'Shipped' | 'Fulfilled' | 'Closed Short' | 'Void' | 'Historical'
    /// </remarks>
    [JsonConverter(typeof(EnumDescriptionConverter))]  //[JsonConverter(typeof(EnumStringConverter<PurchaseOrderStatus>))]
    public enum PurchaseOrderStatus
    {
        [Description("For Calendar")] ForCalendar = 2,  // 1
        [Description("Bid Request")] BidRequest = 10,  // 2
        [Description("Pending Approval")] PendingApproval = 15,  // 3
        [Description("Issued")] Issued = 20,  // 4
        [Description("Picking")] Picking = 30,  // 5
        [Description("Partial")] Partial = 40,  // 6
        [Description("Picked")] Picked = 50,  // 7
        [Description("Shipped")] Shipped = 55,  // 8
        [Description("Fulfilled")] Fulfilled = 60,  // 9
        [Description("Closed Short")] ClosedShort = 70,  // 10
        [Description("Void")] Void = 80,  // 11
        [Description("Historical")] Historical = 95  // 12
    }
}
