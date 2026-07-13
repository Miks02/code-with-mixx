(() => {
    'use strict';

    const OPEN_CLASS = 'translate-x-0';
    const CLOSE_CLASS = '-translate-x-full';

    let sidebar, backdrop, toggle;
    let isOpen = false;

    function open() {
        if (isOpen) return;
        isOpen = true;

        sidebar.classList.replace(CLOSE_CLASS, OPEN_CLASS);
        backdrop.classList.remove('opacity-0', 'pointer-events-none');
        backdrop.classList.add('opacity-100');
        document.body.style.overflow = 'hidden';

        toggle.setAttribute('aria-expanded', 'true');
        toggle.querySelector('i').className = 'fas fa-times text-xl';
    }

    function close() {
        if (!isOpen) return;
        isOpen = false;

        sidebar.classList.replace(OPEN_CLASS, CLOSE_CLASS);
        backdrop.classList.remove('opacity-100');
        backdrop.classList.add('opacity-0', 'pointer-events-none');
        document.body.style.overflow = '';

        toggle.setAttribute('aria-expanded', 'false');
        toggle.querySelector('i').className = 'fas fa-bars text-xl';
    }

    function init() {
        sidebar = document.getElementById('mobile-sidebar');
        backdrop = document.getElementById('sidebar-backdrop');
        toggle = document.getElementById('sidebar-toggle');

        if (!sidebar || !backdrop || !toggle) return;

        toggle.addEventListener('click', () => isOpen ? close() : open(), { passive: true });

        backdrop.addEventListener('click', close, { passive: true });

        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape' && isOpen) close();
        });

        sidebar.querySelectorAll('a[hx-get], a[href]').forEach(link => {
            link.addEventListener('click', close, { passive: true });
        });

        document.addEventListener('htmx:afterSettle', () => {
            if (isOpen) close();
        }, { passive: true });
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init, { once: true });
    } else {
        init();
    }
})();

