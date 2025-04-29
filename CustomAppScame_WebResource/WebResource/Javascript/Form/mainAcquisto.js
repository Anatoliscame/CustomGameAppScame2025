'use STRICT';
if (typeof (CustomAppVideoGame) === "undefined") { CustomAppVideoGame = { __namespace: true }; }
//if (typeof (CustomAppVideoGame.mainAcquisto) == "undefined") { CustomAppVideoGame.mainAcquisto = { __namespace: true }; }
//if (typeof (CustomAppVideoGame.mainAcquisto.OnLoadAcquisto) == "undefined") { CustomAppVideoGame.mainAcquisto.OnLoadAcquisto = { __namespace: true }; }

var FormType =
{
    Create: 1,
    Update: 2
}

const typeAcquisto =
{
    Effetuato: 746200000,
    In_attesa: 746200001,
    Annullato: 746200002
}

CustomAppVideoGame.mainAcquisto = new function () {
    var _self = this;

    _self.onload = function (executionContext) {
        var formContext = executionContext.getFormContext();

        if (formContext.ui.getFormType() == FormType.Create) {


        } if (formContext.ui.getFormType() == FormType.Update) {

            var acquistoId = formContext.data.entity.getId().replace("{", "").replace("}", "");
            if (acquistoId == null) {
                alert("Id non presente");
            }
            formContext.getAttribute("acn_kestatusacquisto").addOnChange(function () { _self.StatusHideEffettuato(executionContext) });

          //  _self.StatusHideEffettuato(executionContext);

            //formContext.getAttribute("acn_kestatusacquisto").addOnChange(_self.OnChangeAcquistoDisableEffetuato);
            _self.OnChangeAcquistoDisableEffetuato(executionContext);

        }
    };

    _self.OnChangeAcquistoDisableEffetuato = function (executionContext) {
        var formContext = executionContext.getFormContext();

        var typeStatusCode = formContext.getAttribute("acn_kestatusacquisto").getValue();

        if (typeStatusCode == typeAcquisto.Effetuato) {
            //formContext.getControl("new_paesescelto").setVisible(true);
            //formContext.getAttribute("new_paesescelto").setValue("Italia");
            formContext.getControl("acn_name").setDisabled(true);
            formContext.getControl("acn_code").setDisabled(true);
            formContext.getControl("acn_account").setDisabled(true);
            formContext.getControl("acn_prodottobrand").setDisabled(true);
            formContext.getControl("acn_iva").setDisabled(true);
            formContext.getControl("acn_fattura").setDisabled(true);
            formContext.getControl("acn_dataacquisto").setDisabled(true);
            formContext.getControl("acn_totale").setDisabled(true);
        } else {
            formContext.getControl("acn_name").setDisabled(false);
            formContext.getControl("acn_code").setDisabled(false);
            formContext.getControl("acn_account").setDisabled(false);
            formContext.getControl("acn_prodottobrand").setDisabled(false);
            formContext.getControl("acn_iva").setDisabled(false);
            formContext.getControl("acn_fattura").setDisabled(false);
            formContext.getControl("acn_dataacquisto").setDisabled(false);
            formContext.getControl("acn_totale").setDisabled(false);
        }
    }

    _self.StatusHideEffettuato = function (executionContext) {
        var formContext = executionContext.getFormContext();

        var StatusOptionSet = formContext.ui.controls.get("acn_kestatusacquisto");
        var status = formContext.getAttribute("acn_kestatusacquisto").getValue();

        if (StatusOptionSet !== null) {
            if (status !== typeAcquisto.Effetuato) {
                StatusOptionSet.removeOption(typeAcquisto.Effetuato);
                StatusOptionSet.addOption({ text: 'Annulato', value: typeAcquisto.Annulato }, 3);
            } else {
                StatusOptionSet.addOption({ text: 'Effetuato', value: typeAcquisto.Effetuato }, 1);
                StatusOptionSet.removeOption(typeAcquisto.Annullato)
            }
        }
    }
}