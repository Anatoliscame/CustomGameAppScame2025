'use strict';

if (typeof CustomApp === "undefined") {
    var CustomApp = { __namespace: true };
}

var FormType =
{
    Create: 1,
    Update: 2
};

const typeAcquisto =
{
    Effetuato: 126400000,
    In_attesa: 126400001,
    Annullato: 126400002
}

CustomApp.mainAcquisto = new function () {
    var _self = this;

    _self.onload = function (executionContext) {
        var formContext = executionContext.getFormContext();

        if (formContext.ui.getFormType() == FormType.Create) {


        } if (formContext.ui.getFormType() == FormType.Update) {

            //var acquistoId = formContext.data.entity.getId().replace("{", "").replace("}", "");

            //formContext.getAttribute("acn_kestatusacquisto").addOnChange(function () { _self.StatusHideEffettuato(executionContext) });

            //formContext.getAttribute("sc_kestatusacquisto").addOnChange(_self.OnChangeAcquistoDisableEffetuato);
            //_self.StatusHideEffettuato(executionContext);

            _self.OnChangeAcquistoDisableEffetuato(executionContext);

        }
    };

    _self.OnChangeAcquistoDisableEffetuato = function (executionContext) {
        var formContext = executionContext.getFormContext();

        var typeStatusCode = formContext.getAttribute("sc_kestatusacquisto").getValue();

        if (typeStatusCode == typeAcquisto.Effetuato) {

            formContext.getControl("sc_name").setDisabled(true);
            formContext.getControl("sc_code").setDisabled(true);
            formContext.getControl("sc_account").setDisabled(true);
            formContext.getControl("sc_dataacquisto").setDisabled(true);
            formContext.getControl("sc_fattura").setDisabled(true);
            formContext.getControl("sc_iva").setDisabled(true);
            formContext.getControl("sc_totale").setDisabled(true);

        } else {

            formContext.getControl("sc_name").setDisabled(false);
            formContext.getControl("sc_code").setDisabled(false);
            formContext.getControl("sc_account").setDisabled(false);
            formContext.getControl("sc_dataacquisto").setDisabled(false);
            formContext.getControl("sc_fattura").setDisabled(false);
            formContext.getControl("sc_iva").setDisabled(false);
            formContext.getControl("sc_totale").setDisabled(false);
        }
    }
    /*
    _self.StatusHideEffettuato = function (executionContext) {
        var formContext = executionContext.getFormContext();
        var status = formContext.getAttribute("sc_kestatusacquisto").getValue();

        if (status !== null) {
            var StatusOptionSet = formContext.getControl("sc_kestatusacquisto");

            if (status !== typeAcquisto.Effetuato) {
                StatusOptionSet.removeOption(typeAcquisto.Effetuato);
                StatusOptionSet.addOption({ text: 'Annullato', value: typeAcquisto.Annullato }, 3);
            } else {
                StatusOptionSet.addOption({ text: 'Effetuato', value: typeAcquisto.Effetuato }, 1);
                StatusOptionSet.removeOption(typeAcquisto.Annullato);
            }
        }
    }*/
}