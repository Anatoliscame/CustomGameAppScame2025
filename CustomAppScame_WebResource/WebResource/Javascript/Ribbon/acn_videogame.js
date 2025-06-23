function apriOrderAcquisto(formContext) {

    var statuscodeVG = formContext.getAttribute("statuscode").getValue();
    if (statuscodeVG !== 746200003) { Xrm.Navigation.openAlertDialog({ text: "Video Game non acquistabile" }); return; }

    var typePiattaformaVG = formContext.getAttribute("acn_typepiattaforma").getValue();
    if (typePiattaformaVG === 746200006) { Xrm.Navigation.openAlertDialog({ text: "Scegli prima una piattaforma d'acquistare" }); return; }


    if (formContext.data.entity.getEntityName() !== "acn_videogame") {
        alert("Questo pulsante funziona solo su VideoGame.");
        return;
    }

    var videoGameId = formContext.data.entity.getId();
    videoGameId = videoGameId.replace("{", "").replace("}", "");
    var videoGameName = formContext.getAttribute("acn_key")?.getValue(); // Nome di VideoGame
    if (!videoGameName) {
        alert("Il nome del videogioco non è disponibile.");
        return;
    }

    var account = formContext.getAttribute("acn_accountid")?.getValue();
    if (!account || account.length === 0) {
        alert("Account non selezionato sul VideoGame.");
        return;
    }
    var accountId = account[0].id.replace("{", "").replace("}", "");


    function generaNumeroCasuale() {
        return Math.floor(Math.random() * 1000000) + 100000;
    }

    var newOrder = {
        "acn_ordername": "Order-" + generaNumeroCasuale(),
        "acn_videogameid@odata.bind": "/acn_videogames(" + videoGameId + ")",
        "acn_accountid@odata.bind": "/accounts(" + accountId + ")"
    };

    Xrm.WebApi.retrieveMultipleRecords("acn_keygame", "?$filter=(_acn_videogame_value eq " + videoGameId + " and acn_statuspresentkeygame eq 746200000)").then(
        function success(results) {
            if (results.entities.length <= 0) {
                Xrm.Navigation.openAlertDialog({ text: "keyGameArray: Chiavi disponibili con un video gioco non ci sono." });
            } else {
                // Aggiungi qua la logica di acn_typepiattaforma
                var keyTrovata = null;

                for (var i = 0; i < results.entities.length; i++) {
                    var keyGame = results.entities[i];
                    if (keyGame.acn_typepiattaforma === typePiattaformaVG) {
                        keyTrovata = true;
                        break;
                    }
                }

                if (!keyTrovata) {
                    Xrm.Navigation.openAlertDialog({ text: "Non ci sono chiavi disponibili per questa piattaforma." });
                    return;
                }
                // Step 1: crea OrderAcquisto
                Xrm.WebApi.createRecord("acn_ordineacquisto", newOrder).then(
                    function (result) {
                        var orderId = result.id;

                        // Step 2: recupera l’OrderAcquisto appena creato (con il campo acn_acquistoid popolato dal plugin)
                        Xrm.WebApi.retrieveRecord("acn_ordineacquisto", orderId, "?$select=acn_ordineacquistoid&$expand=acn_acquistoid($select=acn_acquistoid)").then(
                            function (order) {
                                if (order.acn_acquistoid && order.acn_acquistoid.acn_acquistoid) {
                                    var acquistoId = order.acn_acquistoid.acn_acquistoid;

                                    // Step 3: naviga verso la pagina dell’Acquisto
                                    Xrm.Navigation.openForm({
                                        entityName: "acn_acquisto",
                                        entityId: acquistoId
                                    });
                                    Xrm.Navigation.openAlertDialog({ text: "Ordine creato" });

                                } else {
                                    Xrm.Navigation.openAlertDialog({ text: "Ordine creato, ma non è stato possibile identificare l'Acquisto associato." });
                                }
                            },
                            function (error) {
                                console.error("Errore nel recupero dell’OrderAcquisto: " + error.message);
                            }
                        );
                    },
                    function (error) {
                        console.error("Errore nella creazione dell'OrderAcquisto: " + error.message);
                    }
                );
            }
        },
        function (error) {
            console.error("Errore nel recupero delle chiavi: " + error.message);
        }
    );
}