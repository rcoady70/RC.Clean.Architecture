//
// Script used to process lists
//

//
// Initialize Wire up menu actions. This will "post" the id and the anti forgery token back to the controller
// Example
// <a class="dropdown-item menu-delete" asp-action="Delete" asp-route-id="@item.Id" data-body-message="Are you sure you want to delete @javaScriptEncoder.Encode(item.Name)">Delete</a>
// Use asp-action, asp-route-**,asp-controller etc to build the listUrl.
//
'use strict'
$(document).ready(function () {
    //debugger;

    //Hook up list events, delete, filter etc
    rcList.initListActions();

});

var rcList = {
    deleteForm: null, //Reference to delete form
    //Add delete dialog to body
    addDeleteDialog: function () {
        if ($("#deleteModal").length > 0) {
            console.log("deleteModal already added to form");
            return;
        }
        $('body').append(`<div class="modal fade" id="deleteModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                          <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                              <div class="modal-header">
                                <h5 class="modal-title" id="myModalLabel">Warning</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                              </div>
                              <div class="modal-body delete-modal-body">
                            
                              </div>
                              <div class="modal-footer">
                                <button type="button" class="btn btn-default" data-bs-dismiss="modal">Cancel</button>
                                <button type="button" class="btn btn-danger" id="confirm-delete">Delete</button>
                              </div>
                            </div>
                          </div>
                    </div>`);
        },
    
        //If delete actions bind click event action
        initDeleteListActions: function () {
            //Bind click event to delete actions on list
            $(".evt-menu-delete").on('click.list', (e) => {
                e.preventDefault();
                rcList.listTarget = e.target;

                let Id = $(rcList.listTarget).data('id');
                let bodyMessage = $(rcList.listTarget).data('body-message');

                //Save delete from for later to submit
                rcList.deleteForm = $(e.target)[0].closest("form");
                $(".delete-modal-body").text(bodyMessage);
                $("#deleteModal").modal('show');
            });

            // If confirmation clicked submit form
            $("#confirm-delete").on('click.list', () => {
                rcList.deleteForm.submit();
            });
        },
        //unbind list actions delete, filter button
        unBindListActions: function () {
            $(".evt-menu-delete").unbind("click");
            $("#confirm-delete").unbind("click");
            $("#filterButton").unbind("click");
        },
        //Initialize list actions
        initListActions: function () {
            if ($(".evt-menu-delete").length > 0) {
                //Add delete dialog to page
                rcList.addDeleteDialog();
                //Bind delete button events
                rcList.initDeleteListActions();
            }

            //Set filter badge count
            rcList.setFilterBadge();
            $("#filterButton").on('click.list', function (e) {
                rcList.setFilterBadge();
            });

            //Set toggle icon show addition line on list
            $(".evt-list-col-more").on("click", function (e) {
                rcList.setToggleMoreLines($(this).parent());
            });
        },
        //Set filter badge count
        setFilterBadge: function () {
            let fCount = 0;
            $(".form-control-filter").each(function (index) {
                if ($(this)[0].value.length > 0)
                    fCount++;
                $("#filterBadge").text(fCount.toString());
                if (fCount > 0)
                    $("#collapseFilter").show();
            });
        },

    //Bind toggle more icon 
    setToggleMoreLines: function (event) {
        if ($(event).next().hasClass("list-row-more")) {
            if ($(event).next().is(":visible")) {
                $(event).next().hide();
                $(event).find(".evt-list-col-more-display").addClass("evt-list-col-more");
                $(event).find(".evt-list-col-more-display").removeClass("evt-list-col-more-display");
            }
            else {
                $(event).next().show();
                let mCol = $(event).find(".evt-list-col-more");
                $(mCol).addClass("evt-list-col-more-display");
                $(mCol).removeClass("evt-list-col-more");
            }
        }
    }
}