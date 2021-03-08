"use strict";

/**
 * Create a new shortened URL from input URL.
 */
const CreateNewUrl = (e) => {
    e.preventDefault();

    const url = document.querySelector('input#CreateNewUrl').value.trim();

    if (url === '') {
        alert('URL is required.');
        document.querySelector('input#CreateNewUrl').focus();
    }

    return fetch(`/api/create?url=${encodeURIComponent(url)}`)
        .then(res => {
            switch (res.status) {
                case 200:
                case 400:
                    return res.json();

                default:
                    throw new Error(res.statusText);
            }
        })
        .then(obj => {
            if (obj.message) {
                alert(obj.message);
            }
            else if (obj.url) {
                document.querySelector('span#CreatedUrl')
                    .innerHTML = `Shortened URL: <a href="${obj.url}">${obj.url}</a>`;
            }
            else {
                throw new Error('Not a valid response from API.');
            }
        })
        .catch(err => {
            console.log(err);
            alert(err);
        });
};

/**
 * Reveal a shortened URL.
 */
const RevealUrl = (e) => {
    e.preventDefault();

    const url = document.querySelector('input#RevealUrl').value.trim();

    if (url === '') {
        alert('URL is required.');
        document.querySelector('input#RevealUrl').focus();
    }

    return fetch(`/api/reveal?url=${encodeURIComponent(url)}`)
        .then(res => {
            switch (res.status) {
                case 200:
                case 400:
                case 404:
                    return res.json();

                default:
                    throw new Error(res.statusText);
            }
        })
        .then(obj => {
            if (obj.message) {
                alert(obj.message);
            }
            else if (obj.url) {
                document.querySelector('span#RevealedUrl')
                    .innerHTML = `Original URL: <a href="${obj.url}">${obj.url}</a>`;
            }
            else {
                throw new Error('Not a valid response from API.');
            }
        })
        .catch(err => {
            console.log(err);
            alert(err);
        });
};

/**
 * Init all the things..
 */
(() => {
    document.querySelector('input#Create').addEventListener('click', CreateNewUrl);
    document.querySelector('input#Reveal').addEventListener('click', RevealUrl);
})();