'use strict';

if (typeof CustomApp === "undefined") {
    var CustomApp = { __namespace: true };
}

var FormType =
{
    Create: 1,
    Update: 2
};

const typeProductDigital =
{
    VideoGame: 126400000,
    Licenza_Software: 126400001
};

CustomApp.mainProductDetails = new function () {
    var _self = this;

    _self.onload = function (executionContext) {
        var formContext = executionContext.getFormContext();

        if (formContext.ui.getFormType() == FormType.Create) {


        } else if (formContext.ui.getFormType() == FormType.Update) {

            _self.HideColumnsProductDetails(executionContext);
            formContext.getControl("sc_typeproductdetail").setDisabled(true);

            var typeProductDetailControl = formContext.getControl("sc_typeproductdetail");

            if (typeProductDetailControl != null) {
                typeProductDetailControl.setDisabled(true);
            }
            /*Xrm.Navigation.openAlertDialog({
                title: "Benvenuti a Product Details",
                text: "Ciao, siamo disponibili!"
            });*/
        }
    };


    _self.HideColumnsProductDetails = function (executionContext) {

        var formContext = executionContext.getFormContext();

        var tipoProductDetail = formContext.getAttribute("sc_typeproductdetail");

        if (tipoProductDetail === null) {

            Xrm.Navigation.openAlertDialog({
                title: "tipoProductDetail ",
                text: "optionset non sono disponibili!"
            });
            return;
        }

        switch (tipoProductDetail.getValue()) {

            case typeProductDigital.VideoGame:

                formContext.getControl("sc_tipolicenza").setVisible(false);
                formContext.getControl("sc_numeropostazioni").setVisible(false);
                formContext.getControl("sc_duratamesi").setVisible(false);


                break;

            case typeProductDigital.Licenza_Software:

                formContext.getControl("sc_pg").setVisible(false);
                formContext.getControl("sc_typeexpansion").setVisible(false);
                formContext.getControl("sc_genere").setVisible(false);
                formContext.getControl("sc_datauscita").setVisible(false);

                break;

            default:

                formContext.getControl("sc_pg").setVisible(false);
                formContext.getControl("sc_typeexpansion").setVisible(false);
                formContext.getControl("sc_genere").setVisible(false);
                formContext.getControl("sc_datauscita").setVisible(false);
                formContext.getControl("sc_tipolicenza").setVisible(false);
                formContext.getControl("sc_numeropostazioni").setVisible(false);
                formContext.getControl("sc_duratamesi").setVisible(false);

                break;
        }
    };
};
