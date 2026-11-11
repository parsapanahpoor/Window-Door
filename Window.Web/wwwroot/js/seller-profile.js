(() => {
    const country = document.getElementById('CountryId');
    const state = document.getElementById('StateId');
    const city = document.getElementById('CityId');
    const status = document.getElementById('seller-location-status');
    let pending;

    function reset(select) {
        select.options.length = 1;
        select.selectedIndex = 0;
    }

    async function load(parent, child) {
        pending?.abort();
        reset(child);
        status.textContent = '';
        if (!parent.value) return;

        const request = new AbortController();
        pending = request;
        status.textContent = 'در حال دریافت گزینه‌ها…';
        try {
            const response = await fetch('/Home/LoadCities?stateId=' + encodeURIComponent(parent.value), { signal: request.signal });
            if (!response.ok) throw new Error('Location request failed');
            const result = await response.json();
            if (pending !== request) return;
            for (const item of result.data ?? []) child.add(new Option(item.title, item.id));
            status.textContent = child.options.length === 1 ? 'گزینه‌ای برای این انتخاب وجود ندارد.' : '';
        } catch (error) {
            if (error.name !== 'AbortError' && pending === request)
                status.textContent = 'دریافت گزینه‌ها ممکن نشد. انتخاب را تغییر دهید و دوباره تلاش کنید.';
        }
    }

    country.addEventListener('change', () => { reset(city); load(country, state); });
    state.addEventListener('change', () => load(state, city));
})();

for (const card of document.querySelectorAll('.seller-document')) {
    const input = card.querySelector('input[type="file"]');
    const preview = card.querySelector('img');
    const hint = card.querySelector('.seller-document-hint');
    const defaultHint = hint.textContent;

    preview.addEventListener('load', () => preview.classList.remove('is-unavailable'));
    preview.addEventListener('error', () => preview.classList.add('is-unavailable'));
    if (preview.hasAttribute('src') && preview.complete && !preview.naturalWidth)
        preview.classList.add('is-unavailable');

    input.addEventListener('change', () => {
        hint.textContent = input.files[0]?.name ?? defaultHint;
    });
}
