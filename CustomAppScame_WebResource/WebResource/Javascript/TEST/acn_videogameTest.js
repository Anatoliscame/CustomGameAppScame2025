function apriOrderAcquisto(formContext) {

    if (formContext.data.entity.getEntityName() !== "acn_videogame") {
        alert("Questo pulsante funziona solo su VideoGame.");
        return;
    }

    // ID del VideoGame attualmente aperto
    var videoGameId = formContext.data.entity.getId();
    videoGameId = videoGameId.replace("{", "").replace("}", "");
    var videoGameName = formContext.getAttribute("acn_key")?.getValue(); // Nome di VideoGame

    if (!videoGameName) {
        alert("Il nome del videogioco non è disponibile. Assicurati che sia compilato e visibile nel form.");
        return;
    }
    // ID dell'utente loggato (SystemUser)
    var userId = Xrm.Utility.getGlobalContext().userSettings.userId;
    userId = userId.replace("{", "").replace("}", "");

    function generaNumeroCasuale() {
        return Math.floor(Math.random() * 1000000) + 100000;  // Un numero casuale tra 100000 e 999999
    }

    // Recupera il SystemUser con l'account collegato
    Xrm.WebApi.retrieveRecord("systemuser", userId, "?$select=fullname&$expand=acn_accountid($select=accountid,name)").then(
        function success(result) {
            if (result.acn_accountid) {  // Controlla solo se new_accountid è presente
                var accountId = result.acn_accountid.accountid;
                //var accountName = result.acn_accountid.name;

                // Genera un numero casuale per acn_ordername
                var randomOrderNumber = generaNumeroCasuale();

                // Apre il form di OrderAcquisto precompilato
                var entityFormOptions = {
                    entityName: "acn_ordineacquisto",
                    useQuickCreateForm: false
                };

                //var formParameters = {};

                var formParameters = {
                    acn_videogameid: {
                        id: videoGameId,
                        name: videoGameName,
                        entityType: "acn_videogame"
                    },
                    acn_ordername: "Order-" + randomOrderNumber
                    //formParameters["acn_accountid"] = accountId;
                };

                Xrm.Navigation.openForm(entityFormOptions, formParameters).then(
                    function (lookup) {
                        console.log("Modulo OrderAcquisto aperto.");
                    },
                    function (error) {
                        console.error("Errore apertura modulo: " + error.message);
                    }
                );
            } else {
                Xrm.Navigation.openAlertDialog({ text: "L'utente corrente non ha un Account associato. Contatta l'amministratore." });
            }
        },
        function (error) {
            console.error("Errore nel recupero dell'utente: " + error.message);
            Xrm.Navigation.openAlertDialog({ text: "Errore nel recupero dell'utente." });
        }
    );
}