# API ENDPOINTS CHECKLIST

**GET /api/images**

Returns all images, paginated (10 per page), ordered by posting date.

**GET /api/images/{id}**

Returns the details of a specific image by ID, including the URL, user name, user ID, and tags.

Returns a 404 if the image does not exist.

**GET /api/images/byTag**

Returns images filtered by a given tag, paginated (10 per page), and ordered by posting date.

Returns 404 if no images match the given tag.

**GET /api/images/populartags**

Returns the top 5 popular tags based on their frequency of use.

**POST /api/users**

Adds a new user to the database.

Validates that the user’s email is unique and the required fields are provided.

**POST /api/users/{id}/image**

Adds an image to a specified user and generates tags using ImageHelper.GetTags.

Returns the user’s details, including the last 10 images.

**GET /api/users/{id}**

Retrieves a user's details, including the last 10 images.

**GET /api/users/{id}/images**

Returns all images of a given user, paginated (10 per page), ordered by posting date.

**DELETE /api/users/{id}**

Deletes a user and all their associated image
