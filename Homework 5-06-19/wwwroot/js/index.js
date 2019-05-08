$(() => {

    $("#confirm").on('click', function () {

        const button = $(this);
        const id = button.data("id");
        $.post("/home/confirm", { id }, function (counts) {
            $("#declined-count").text(`Declined(${counts.declined})`);
            $("#confirmed-count").text(`Confirmed(${counts.confirmed})`);
            $("#pending-count").text(`Pending(${counts.pending})`); 
        });
    });

        $("#decline").on('click', function () { 
            const button = $(this);
            const id = button.data("id");
            $.post("/home/Decline", { id }, function (counts) {                
                $("#declined-count").text(`Declined(${counts.declined})`);
                $("#confirmed-count").text(`Confirmed(${counts.confirmed})`);
                $("#pending-count").text(`Pending(${counts.pending})`);
            });
        });

    });       
        


     