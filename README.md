# FancyRedirect

Fancy Redirect (frdr.it) is a simple URL shortener service. Enter the URL you want shortened and it should give you a frdr.it version of it. Below you can also enter the frdr.it shortened URL and reveal the URL it will redirect too.

<https://frdr.it/>

## API
Fancy Redirect has an API you can use.

### Create
`GET https://frdr.it/api/create?url=<your-url-encoded-url-here>`

Example:

`GET https://frdr.it/api/create?url=https://microsoft.com/` will create a shortened URL looking like this: `https://frdr.it/r/ae`.

Possible Response Code:
* 200 Ok. Everything is ok. The payload will include the shortened URL in the `url` property.
* 400 Bad Request. This will include a payload message explaining what was wrong in the `message` property.
* 429 Too Many Requests. The API is setup to allow 1 request every 2 seconds.

### Reveal
`GET https://frdr.it/api/reveal?url=<the-shortened-url-here>`

Example:
`GET https://frdr.it/api/reveal?url=https://frdr.it/e/ae` will reveal the full URL which is will redirect to if used, using the example above: `https://microsoft.com/`.

Possible Response Code:
* 200 Ok. Everything is ok. The payload will include the full URL of the original resource in the `url` property.
* 400 Bad Request. This will include a payload message explaining what was wrong in the `message` property.
* 404 Not Found. The shortened URL given was not found.
* 429 Too Many Requests. The API is setup to allow 1 request every 2 seconds.

## Problems?
Having problems using the service, either the API or the interface, feel free to contact me or create an issue here on the GitHub repo.

## Credit
Icon made by [DinosoftLabs](https://www.flaticon.com/authors/dinosoftlabs) from [Flaticon](https://www.flaticon.com/)
