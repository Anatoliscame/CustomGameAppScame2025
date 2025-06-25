'use STRICT';
if (typeof (CustomAppVideoGame) === "undefined") { CustomAppVideoGame = { __namespace: true }; }

var FormType =
{
    Create: 1,
    Update: 2
}

const typeVideogioco =
{
    BaseGame: 746200000,
    DLC: 746200001,
    Remastered: 746200002,
    Espansione: 746200003,
    Altro: 746200004
}

CustomAppVideoGame.mainVideoGame = new function () {
    var _self = this;

    _self.onload = function (executionContext) {
        var formContext = executionContext.getFormContext();

        if (formContext.ui.getFormType() == FormType.Create) {


        } if (formContext.ui.getFormType() == FormType.Update) {

            //var acquistoId = formContext.data.entity.getId().replace("{", "").replace("}", "");
            _self.OnChangeVideoGameGenereVisibly(executionContext);
            _self.StatusHideTypeVideoGame(executionContext);
        }
    };

    _self.OnChangeVideoGameGenereVisibly = function (executionContext) {
        var formContext = executionContext.getFormContext();

        var typeStatusCode = formContext.getAttribute("acn_tipovideogioco").getValue();

        if (typeStatusCode == typeVideogioco.BaseGame
            || typeStatusCode == typeVideogioco.Espansione
            || typeStatusCode == typeVideogioco.Remastered) {

            formContext.getControl("acn_parentvideogameid").setVisible(false);
        } else {
            formContext.getControl("acn_parentvideogameid").setVisible(true);
            formContext.getControl("acn_parentvideogameid").setDisabled(true);
        }
    }

    _self.StatusHideTypeVideoGame = function (executionContext) {
        var formContext = executionContext.getFormContext();

        var typeStatusCode = formContext.getAttribute("acn_tipovideogioco").getValue();
        if (typeStatusCode !== null) {
        var StatusOptionSet = formContext.getControl("acn_tipovideogioco");

        switch (typeStatusCode)
        {
            case typeVideogioco.BaseGame:
                StatusOptionSet.removeOption(typeVideogioco.DLC);
                StatusOptionSet.removeOption(typeVideogioco.Remastered);
                StatusOptionSet.removeOption(typeVideogioco.Altro);
                break;
            case typeVideogioco.Espansione:
                StatusOptionSet.removeOption(typeVideogioco.DLC);
                StatusOptionSet.removeOption(typeVideogioco.Remastered);
                StatusOptionSet.removeOption(typeVideogioco.Altro);
                break;
            case typeVideogioco.DLC:
                StatusOptionSet.removeOption(typeVideogioco.BaseGame);
                StatusOptionSet.removeOption(typeVideogioco.Remastered);
                StatusOptionSet.removeOption(typeVideogioco.Espansione);
                StatusOptionSet.removeOption(typeVideogioco.Altro);
                break;
            default:
                break;
        }
        }
    }
}