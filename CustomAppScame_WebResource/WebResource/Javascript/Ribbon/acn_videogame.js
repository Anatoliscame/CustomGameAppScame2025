function apriOrderAcquisto(formContext) {

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

    var userId = Xrm.Utility.getGlobalContext().userSettings.userId;
    userId = userId.replace("{", "").replace("}", "");

    function generaNumeroCasuale() {
        return Math.floor(Math.random() * 1000000) + 100000;
    }

    Xrm.WebApi.retrieveRecord("systemuser", userId, "?$select=fullname&$expand=acn_accountid($select=accountid,name)").then(
        function (userResult) {
            if (userResult.acn_accountid) {
                var accountId = userResult.acn_accountid.accountid;

                var newOrder = {
                    "acn_ordername": "Order-" + generaNumeroCasuale(),
                    "acn_videogameid@odata.bind": "/acn_videogames(" + videoGameId + ")",
                    "acn_accountid@odata.bind": "/accounts(" + accountId + ")"
                };

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
            } else {
                Xrm.Navigation.openAlertDialog({ text: "L'utente non ha un Account associato." });
            }
        },
        function (error) {
            console.error("Errore nel recupero dell’utente: " + error.message);
        }
    );
}