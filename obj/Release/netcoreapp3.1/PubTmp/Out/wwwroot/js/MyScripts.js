GlobalUrl = "http://localhost:30198/";

function GetCart() {
   
  // $("#CartData").load("/Sales/RendorCart");
}

$(document).ready(function () {
    var uid = $('#loggedUserId').val();
    GetNotifications(uid);
    GetMenus(uid);
    GetCart();
});

function AddToCart(prid) {
    var userid = $('#cuid').val();
    if (userid == 0 || userid == undefined)
    {
        toastr["error"]("Select Customer first to add to cart!");
    }
    else
    {
        swal("Are you sure to add this to Cart ?", {
            buttons: {
                yes: {
                    text: "Yes",
                    value: "yes"
                },
                no: {
                    text: "No",
                    value: "no"
                }
            }
        }).then((value) => {
            if (value === "yes")
            {
                    item = $('#prdQty').val();
                    avqty = $('#availableQty').val();
             
                    $.ajax({
                        url: GlobalUrl + "Sales/AddToCart?inventoryid=" + prid + "&qty=" + item,
                        type: 'post',
                        data: '{}',
                        success: function (result) {
                            if (result.result.statusCode == 1) {
                                toastr["success"]("Successuflly Added!");
                                window.location.href = window.location.href;
                            } else {
                                toastr["error"](result.result.statusMessage);
                            }
                            
                        }
                    });

                 
                
            }
            else {
                toastr["error"]("Add to cart Cancelled!")
            }
            return false;
        });
    }



}


function GetNotifications(uid) {
    
    $("#NotificationsList").load("/Home/GetMyNotifications?uid=" + uid);
}
function GetMenus(uid) {
    $("#sideBarMenu").load("/Home/GetMenuItems?uid=" + uid);
}
