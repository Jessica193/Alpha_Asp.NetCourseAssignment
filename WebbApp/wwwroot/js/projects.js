
const previewSize = 150;
function filterProjects(status) {
    fetch(`/Projects/GetProjectsByStatus?status=${status}`)
        .then(response => response.text())
        .then(html => {
            document.getElementById('project-container').innerHTML = html;


            //functions are located in site.js
            initializeDropdowns();
            initializeOpenModals();
            initializeProjectModals();
            initializeCloseModals();
            initializeImagePreview(previewSize);
            initializeModalFormSubmission(); 
            initializeTomSelects();

        })
}