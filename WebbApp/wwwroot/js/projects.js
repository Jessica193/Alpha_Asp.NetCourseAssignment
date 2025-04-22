function filterProjects(status) {
    fetch(`/Projects/GetProjectsByStatus?status=${status}`)
        .then(response => response.text())
        .then(html => {
            document.getElementById('project-container').innerHTML = html;
        })
}