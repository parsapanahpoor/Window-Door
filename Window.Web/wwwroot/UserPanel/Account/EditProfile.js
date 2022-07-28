const countryEl = document.getElementById('CountryId');
const stateEl = document.getElementById('StateId');
const cityEl = document.getElementById('CityId');

$(countryEl).on('change',
    function (event) {
        stateEl.innerHTML = "";
        cityEl.innerHTML = "";
        stateEl.setAttribute('disabled', '');
        cityEl.setAttribute('disabled', '');

        const countryId = event.currentTarget.value;

        if (!countryId) {
            return;
        }

        $.ajax({
            method: 'get',
            url: '/Location/GetLocationsByParentId/' + countryId,
            success: function (response) {
                stateEl.removeAttribute('disabled');
                createLocationsElement(response.data, stateEl);
            }
        });
    });

$(stateEl).on('change',
    function (event) {
        cityEl.innerHTML = "";
        cityEl.setAttribute('disabled', '');

        const stateId = event.currentTarget.value;

        if (!stateId) {
            return;
        }

        $.ajax({
            method: 'get',
            url: '/Location/GetLocationsByParentId/' + stateId,
            success: function (response) {
                cityEl.removeAttribute('disabled');
                createLocationsElement(response.data, cityEl);
            }
        });
    });

function createLocationsElement(data, insertEl) {
    for (let location of data) {
        let optionEl = document.createElement('option');
        optionEl.setAttribute('value', location.locationId);
        optionEl.innerHTML = location.name;

        insertEl.appendChild(optionEl);
    }
    $(insertEl).trigger('change');
}





/*$('#select-avatar-button').on('click', function (event) {
    $('#Avatar').click();
});*/

$('#Avatar').on('change', function (event) {
    const file = Avatar.files[0];
    if (file) {
        $('#user-avatar-preview').attr('src', URL.createObjectURL(file));
    }
});