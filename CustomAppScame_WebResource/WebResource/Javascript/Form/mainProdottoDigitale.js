'use strict';

if (typeof CustomApp === "undefined") {
    var CustomApp = { __namespace: true };
}

var FormType =
{
    Create: 1,
    Update: 2
};

const typePiattaformaDigital =
{
    Steam: 126400000,
    EA: 126400001,
    Ubisoft: 126400002,
    Psn: 126400003,
    Microsoft_Xbox: 126400004,
    Epic_Games: 126400005,
    Nintendo_eShop: 126400006,
    Netflix: 126400007,
    Sportify: 126400008,
    Amazon: 126400009,
    Microsoft: 126400010,
    Adobe: 126400011,
    Autodesk: 126400012,
    Norton: 126400013,
    Telegram: 126400014,
    Reseller: 126400015
};

const typeProductDigital =
{
    VideoGame: 126400000,
    Licenza_Software: 126400001
};

CustomApp.mainProdottoDigitale = new function () {
    var _self = this;
    var _redirectAlreadyDone = false;

    _self.onload = function (executionContext) {
        var formContext = executionContext.getFormContext();

        var productDetailsValue = formContext.getAttribute("sc_productdetails") != null
            ? formContext.getAttribute("sc_productdetails").getValue()
            : null;
        var productDetailsControl = formContext.getControl("sc_productdetails");


        if (formContext.ui.getFormType() == FormType.Create) {

            formContext.data.entity.addOnPostSave(_self.OpenSpecificEntityAfterCreate);

            if (productDetailsControl != null) {
                productDetailsControl.setVisible(false);
            }

        } else if (formContext.ui.getFormType() == FormType.Update) {


            _self.HideTypePiattaforma(executionContext);

            if (productDetailsControl != null) {

                if (productDetailsValue != null && productDetailsValue.length > 0) {
                    productDetailsControl.setVisible(true);
                } else {
                    productDetailsControl.setVisible(false);
                }
            }

            _self.SetFieldsDisabled(executionContext);
        }
    };

    _self.VisiblyMessage = function (executionContext) {
        var formContext = executionContext.getFormContext();

        Xrm.Navigation.openAlertDialog({
            title: "Aggiornamento",
            text: "Ciao, questo è un alert!"
        });
    };


    _self.HideTypePiattaforma = function (executionContext) {

        var formContext = executionContext.getFormContext();

        var tipoProductAttr = formContext.getAttribute("sc_tipoprodottodigitale");
        var piattaformaAttr = formContext.getAttribute("sc_piattaformaprodotttodigitale");
        var piattaformaControl = formContext.getControl("sc_piattaformaprodotttodigitale");

        if (tipoProductAttr === null || piattaformaAttr === null || piattaformaControl === null) {

            Xrm.Navigation.openAlertDialog({
                title: "tipoProductAttr e piattaformaAttr",
                text: "Due optionset non sono disponibili!"
            });

            return;
        }

        var tipoProduct = tipoProductAttr.getValue();
        var attualeValuePiattaforma = piattaformaAttr.getValue();

        var allowedOptions = [];

        switch (tipoProduct) {

            case typeProductDigital.VideoGame:

                allowedOptions = [
                    { text: "Steam", value: typePiattaformaDigital.Steam },
                    { text: "EA", value: typePiattaformaDigital.EA },
                    { text: "Ubisoft", value: typePiattaformaDigital.Ubisoft },
                    { text: "PSN", value: typePiattaformaDigital.Psn },
                    { text: "Microsoft Xbox", value: typePiattaformaDigital.Microsoft_Xbox },
                    { text: "Epic Games", value: typePiattaformaDigital.Epic_Games },
                    { text: "Nintendo eShop", value: typePiattaformaDigital.Nintendo_eShop }
                ];

                break;

            case typeProductDigital.Licenza_Software:

                allowedOptions = [
                    { text: "Microsoft", value: typePiattaformaDigital.Microsoft },
                    { text: "Adobe", value: typePiattaformaDigital.Adobe },
                    { text: "Autodesk", value: typePiattaformaDigital.Autodesk },
                    { text: "Norton", value: typePiattaformaDigital.Norton },
                    { text: "Amazon", value: typePiattaformaDigital.Amazon },
                    { text: "Reseller", value: typePiattaformaDigital.Reseller }
                ];

                break;

            default:
                break;
        }

        piattaformaControl.clearOptions();

        for (var i = 0; i < allowedOptions.length; i++) {
            piattaformaControl.addOption(allowedOptions[i]);
        }

        var isAttualeValueAllowed = false;

        for (var j = 0; j < allowedOptions.length; j++) {

            if (allowedOptions[j].value === attualeValuePiattaforma) {
                isAttualeValueAllowed = true;
                break;
            }
        }

        if (attualeValuePiattaforma !== null && isAttualeValueAllowed === false) {
            piattaformaAttr.setValue(null);
            piattaformaAttr.fireOnChange();
        }
    };

    _self.SetFieldsDisabled = function (executionContext) {
        var formContext = executionContext.getFormContext();

        formContext.getControl("sc_tipoprodottodigitale").setDisabled(true);
        formContext.getControl("sc_prodottodigitale").setDisabled(true);
        formContext.getControl("sc_codiceprodotto").setDisabled(true);

    };


    _self.OpenSpecificEntityAfterCreate = function (executionContext) {
        var formContext = executionContext.getFormContext();

        if (_redirectAlreadyDone === true) {
            return;
        }

        var tipoProductAttr = formContext.getAttribute("sc_tipoprodottodigitale");


        if (tipoProductAttr == null) {
            Xrm.Navigation.openAlertDialog({
                title: "Campi mancanti",
                text: "Tipo prodotto, piattaforma o nome prodotto non disponibili."
            });
            return;
        }

        var tipoProduct = tipoProductAttr.getValue();

        if (tipoProduct == null) {
            return;
        }

        var prodottoDigitaleId = formContext.data.entity.getId();

        if (prodottoDigitaleId == null || prodottoDigitaleId === "") {
            return;
        }

        prodottoDigitaleId = prodottoDigitaleId.replace("{", "").replace("}", "");

        var data = {};
        var entityName = "sc_productdetails";
        // Campi da compilare nel record figlio
        data["sc_name"] = formContext.getAttribute("sc_prodottodigitale").getValue() + " - " + formContext.getAttribute("sc_codiceprodotto").getValue();
        data["sc_typeproductdetail"] = tipoProduct;
        // Lookup verso Prodotto Digitale
        // ATTENZIONE: "sc_prodottodigitales" deve essere il nome EntitySetName/plurale Web API della tabella Prodotto Digitale

        _redirectAlreadyDone = true;

        Xrm.Utility.showProgressIndicator("Creazione record specifico in corso...");

        Xrm.WebApi.createRecord(entityName, data).then(
            function success(result) {

                var productDetailsId = result.id.replace("{", "").replace("}", "");

                var updateProdottoDigitale = {};

                // Inserisce productdetails CREATO su entita Prodotto Digitale
                updateProdottoDigitale["sc_productdetails@odata.bind"] = "/sc_productdetailses(" + productDetailsId + ")";

                Xrm.WebApi.updateRecord("sc_prodottodigitale",prodottoDigitaleId, updateProdottoDigitale).then(
                    function successUpdate() {

                        Xrm.Utility.closeProgressIndicator();

                        Xrm.Navigation.openForm({
                            entityName: "sc_prodottodigitale",
                            entityId: prodottoDigitaleId
                        });
                    },
                    function (error) {

                        Xrm.Utility.closeProgressIndicator();

                        _redirectAlreadyDone = false;

                        Xrm.Navigation.openAlertDialog({
                            title: "Errore aggiornamento Prodotto Digitale",
                            text: error.message
                        });
                    }
                );
            },
            function (error) {

                Xrm.Utility.closeProgressIndicator();

                _redirectAlreadyDone = false;

                Xrm.Navigation.openAlertDialog({
                    title: "Errore creazione Product Details",
                    text: error.message
                });
            }
        );
    };
};