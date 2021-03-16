"use strict";

/**
 * Create a new shortened URL from input URL.
 */
const CreateNewUrl = (e) => {
    e.preventDefault();

    const url = document.querySelector('input#CreateNewUrl').value.trim();
    const status = document.querySelector('span#CreatedUrl');

    if (url === '') {
        status.classList.add('error');
        status.classList.remove('success');
        status.innerHTML = `URL is required.`;

        document.querySelector('input#CreateNewUrl').focus();

        return null;
    }

    return fetch(`/api/create?url=${encodeURIComponent(url)}`)
        .then(res => {
            switch (res.status) {
                case 200:
                case 400:
                case 429:
                    return res.json();

                default:
                    throw new Error(res.statusText);
            }
        })
        .then(obj => {
            if (obj.message) {
                throw new Error(obj.message);
            }
            else if (obj.url) {
                status.classList.remove('error');
                status.classList.add('success');
                status.innerHTML = `Shortened URL: <a href="${obj.url}">${obj.url}</a>`;
            }
            else {
                throw new Error('Not a valid response from API.');
            }
        })
        .catch(err => {
            status.classList.add('error');
            status.classList.remove('success');
            status.innerHTML = `${err}`;
        });
};

/**
 * Reveal a shortened URL.
 */
const RevealUrl = (e) => {
    e.preventDefault();

    const url = document.querySelector('input#RevealUrl').value.trim();
    const status = document.querySelector('span#RevealedUrl');

    if (url === '') {
        status.classList.add('error');
        status.classList.remove('success');
        status.innerHTML = `URL is required.`;

        document.querySelector('input#RevealUrl').focus();

        return null;
    }

    return fetch(`/api/reveal?url=${encodeURIComponent(url)}`)
        .then(res => {
            switch (res.status) {
                case 200:
                case 400:
                case 404:
                case 429:
                    return res.json();

                default:
                    throw new Error(res.statusText);
            }
        })
        .then(obj => {
            if (obj.message) {
                throw new Error(obj.message);
            }
            else if (obj.url) {
                status.classList.remove('error');
                status.classList.add('success');
                status.innerHTML = `Original URL: <a href="${obj.url}">${obj.url}</a>`;
            }
            else {
                throw new Error('Not a valid response from API.');
            }
        })
        .catch(err => {
            status.classList.add('error');
            status.classList.remove('success');
            status.innerHTML = `${err}`;
        });
};

/**
 * Init all the things..
 */
(() => {
    document.querySelector('input#Create').addEventListener('click', CreateNewUrl);
    document.querySelector('input#Reveal').addEventListener('click', RevealUrl);
})();