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

    _self.onload = function (executionContext) {
        var formContext = executionContext.getFormContext();

        if (formContext.ui.getFormType() == FormType.Create) {

            _self.OnChangeHideTypePiattaforma(executionContext);

        } else if (formContext.ui.getFormType() == FormType.Update) {

            _self.SetFieldsDisabled(executionContext);

            // _self.VisiblyMessage(executionContext);
        }
    };

    _self.VisiblyMessage = function (executionContext) {
        var formContext = executionContext.getFormContext();

        Xrm.Navigation.openAlertDialog({
            title: "Aggiornamento",
            text: "Ciao, questo è un alert!"
        });
    };

    _self.OnChangeHideTypePiattaforma = function (executionContext) {
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

        var tipoProductControl = formContext.getControl("sc_tipoprodottodigitale");
        var piattaformaControl = formContext.getControl("sc_piattaformaprodotttodigitale");

        if (tipoProductControl != null) {
            tipoProductControl.setDisabled(true);
        }

        if (piattaformaControl != null) {
            piattaformaControl.setDisabled(true);
        }
    };
};