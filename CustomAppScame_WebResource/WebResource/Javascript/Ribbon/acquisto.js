function checkAcquistoStatusVisibility(formContext) {
    var kestatusacquisto = formContext.getAttribute("acn_kestatusacquisto").getValue(); // Stato dell'acquisto

        // Verifica se lo stato è "In attesa" (746200002) e rende visibile il pulsante
        if (kestatusacquisto === 746200001) { // 746200002 è "In attesa"
            return true;  // Rende visibile il pulsante
        }
        return false;  // Rende invisibile il pulsante se lo stato è diverso
}

function completaAcquistoFattura(formContext) {
    var acquistoId = formContext.data.entity.getId();
    acquistoId = acquistoId.replace("{", "").replace("}", "");
    // Esempio: cambiamo lo statuscode a "Effettuato"

    var statusAcquisto = formContext.getAttribute("acn_kestatusacquisto").getValue();

    // 746200002 è "In attesa"
    if (statusAcquisto !== 746200001) { Xrm.Navigation.openAlertDialog({ text: "Acquisto Carrello non deve essere manuelmente cambiato per rispettare le regole" }); return; }

    var accountidLookup = formContext.getAttribute("acn_account");

    if (!accountidLookup || !accountidLookup.getValue() || accountidLookup.getValue().length === 0) { Xrm.Navigation.openAlertDialog({ text: "accountidLookup è nullo o vuoto" }); return; }

    var fetchUrl = "<fetch mapping='logical' version='1.0' output-format='xml-platform' distinct='false' >" +
        "<entity name='acn_ordineacquisto'>" +
        "<filter type='and'>" +
        "<condition attribute='acn_acquistoid' operator='eq' value='" + acquistoId + "' />" +
        "</filter>" +
        "<attribute name='acn_keygamecode' />" +
        "</entity>" +
        "</fetch>";

    var path = "?fetchXml=" + fetchUrl;
    var keygamecode = null;
    // Esegui la chiamata asincrona per recuperare i record
    Xrm.WebApi.retrieveMultipleRecords("acn_ordineacquisto", path).then(
        function success(result) {
            if (result.entities.length > 0) {
                // Cicla attraverso i record di ordineacquisto recuperati
                for (var i = 0; i < result.entities.length; i++) {
                    var ordineAcquisto = result.entities[i];
                    //console.log("Ordine Acquisto ID: " + ordineAcquisto.acn_ordineacquistoid);
                    keygamecode = ordineAcquisto.acn_keygamecode;

                    // Verifica se 'acn_keygamecode' è vuoto o nullo
                    if (!keygamecode || keygamecode.trim() === "") {
                        console.log("KeyGame Name non valorizzato.");
                        Xrm.Navigation.openAlertDialog({ text: "KeyGame Name non valorizzato." });
                        //return;
                        // break; // Esci dal ciclo se la condizione è soddisfatta
                    } else {
                        //console.log("KeyGame Name: " + keygamecode);
                        Xrm.Navigation.openAlertDialog({ text: "KeyGame Name:", keygamecode });
                        var updateData = {
                            "acn_kestatusacquisto": 746200000 // Metti il valore corretto per "Effettuato"
                        };

                        Xrm.WebApi.updateRecord("acn_acquisto", acquistoId, updateData).then(
                            function success() {
                                console.log("Acquisto aggiornato, plugin dovrebbe partire.");

                                var prodottobrand = formContext.getControl("acn_prodottobrand");
                                if (prodottobrand) {
                                    prodottobrand.setDisabled(true);
                                }
                               /* formContext.getControl("acn_name").setDisabled(true);
                                formContext.getControl("acn_code").setDisabled(true);
                                formContext.getControl("acn_account").setDisabled(true);
                                formContext.getControl("acn_iva").setDisabled(true);
                                formContext.getControl("acn_fattura").setDisabled(true);
                                formContext.getControl("acn_dataacquisto").setDisabled(true);
                                formContext.getControl("acn_totale").setDisabled(true);*/
                                Xrm.Navigation.openAlertDialog({ text: "Acquisto completato." });
                            },
                            function (error) {
                                console.error("Errore nell'aggiornamento dell'acquisto: ", error.message);
                                Xrm.Navigation.openAlertDialog({ text: "Errore nell'aggiornamento dell'acquisto:" });
                            }
                        );
                    }
                }
            } else {
                console.log("Nessun ordine di acquisto trovato per l'Acquisto ID: " + acquistoId);
            }
        },
        function (error) {
            console.error("Errore nel recupero degli ordini di acquisto: " + error.message);
        }
    );
}