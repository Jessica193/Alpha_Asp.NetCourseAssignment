document.addEventListener('DOMContentLoaded', () => {
    const previewSize = 150;

    initializeTomSelects();
    initializeDropdowns();
    initializeOpenModals();
    initializeModalFormSubmission();
    initializeMemberModals();
    initializeClientModals();
    initializeImagePreview(150);
    initializeProjectModals();
    initializeCloseModals();
    initializeDarkMode()
});


//1. Ability to select multiple members in the project modal
function initializeTomSelects() {
    document.querySelectorAll('.js-member-select').forEach(el => {

        if (el.tomselect) {
            el.tomselect.destroy();
        }

        if (!el.tomselect) {
            new TomSelect(el, {
                placeholder: 'Search and select members',
                plugins: ['remove_button'],
                render: {
                    option: function (data, escape) {
                        return `
                        <div class="ts-option-with-avatar">
                            <img src="${escape(data.image)}" class="avatar" />
                            <span>${escape(data.text)}</span>
                        </div>`;
                    },
                    item: function (data, escape) {
                        return `
                        <div class="ts-item-with-avatar">
                            <img src="${escape(data.image)}" class="avatar avatar-sm" />
                            <span>${escape(data.text)}</span>
                        </div>`;
                    }
                },
                onInitialize: function () {
                    const select = this.input.closest('select');
                    const options = select.querySelectorAll('option');

                    options.forEach(opt => {
                        const option = this.options[opt.value];
                        if (option && opt.dataset.image) {
                            option.image = opt.dataset.image;
                        }
                    });
                }
            });
        }
    });
}



//2. show dropdowns when clicking button
function initializeDropdowns() {
    const dropdownButtons = document.querySelectorAll('[data-type="dropdown"]');

    document.addEventListener('click', function (event) {
        let clickedDropdownButton = null;

        dropdownButtons.forEach(dropdownButton => {
            const targetId = dropdownButton.getAttribute('data-target');
            const targetDropdownMenu = document.querySelector(targetId);

            if (dropdownButton.contains(event.target)) {
                clickedDropdownButton = targetDropdownMenu;

                document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
                    if (openDropdown !== targetDropdownMenu) {
                        openDropdown.classList.remove('dropdown-show');
                    }
                });

                targetDropdownMenu.classList.toggle('dropdown-show');
            }
        });

        if (!clickedDropdownButton && !event.target.closest('.dropdown')) {
            document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
                openDropdown.classList.remove('dropdown-show');
            });
        }
    });
}

// 3. Open modals
function initializeOpenModals() {
    const initializedQuills = new Set(); // För att undvika dubbelinitiering

    document.querySelectorAll('[data-modal="true"]').forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target');
            const modal = document.querySelector(modalTarget);
            if (modal) {
                modal.style.display = 'flex';

                // Kolla om den här modalen har en Quill-editor
                const wysiwygEditor = modal.querySelector('[id$="wysiwyg-editor"]');
                const wysiwygToolbar = modal.querySelector('[id$="wysiwyg-toolbar"]');
                const textarea = modal.querySelector('textarea');

                if (wysiwygEditor && wysiwygToolbar && textarea) {
                    const editorId = `#${wysiwygEditor.id}`;
                    const toolbarId = `#${wysiwygToolbar.id}`;
                    const textareaId = `#${textarea.id}`;

                    // Initiera bara om det inte redan är gjort
                    if (!initializedQuills.has(editorId)) {
                        initWysiwyg(
                            editorId,
                            toolbarId,
                            textareaId,
                            textarea.value
                        );
                        initializedQuills.add(editorId);
                    }
                }
            }
        });
    });
}


//4. Handle form submission
function initializeModalFormSubmission() {
    const modalForms = document.querySelectorAll('form[data-modal-form="true"]');
    modalForms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault();
            clearErrorMessages(form);

            const formData = new FormData(form);

            try {
                const res = await fetch(form.action, {
                    method: 'POST',
                    body: formData
                });

                if (res.ok) {
                    const modal = form.closest('.modal');
                    if (modal)
                        modal.style.display = 'none';

                    window.location.reload();
                }
                else if (res.status === 400) {
                    const data = await res.json();

                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            let input = form.querySelector(`[name="${key}"]`);
                            if (input) {
                                input.classList.add('input-validation-error');
                            }

                            const span = form.querySelector(`[data-valmsg-for="${key}"]`);
                            if (span) {
                                span.innerText = data.errors[key].join('\n');
                                span.classList.add('field-validation-error');
                            }
                        });
                    }
                }
            } catch (e) {
                console.error('Error submitting the form', e);
            }
        });
    });
}


//5. Pre-filling the form when editing member. Generated by chatGPT4o
function initializeMemberModals() {
    document.querySelectorAll('[data-member-id]').forEach(button => {
        button.addEventListener('click', async () => {
            const memberId = button.getAttribute('data-member-id');
            const modalId = button.getAttribute('data-target');
            const modal = document.querySelector(modalId);
            const form = modal.querySelector('form');

            try {
                const response = await fetch(`/Members/GetMember?id=${memberId}`);
                if (!response.ok) throw new Error('Member could not be fetched');

                const data = await response.json();

                //pre-filling form
                form.querySelector('[name="Id"]').value = data.id;
                form.querySelector('[name="FirstName"]').value = data.firstName;
                form.querySelector('[name="LastName"]').value = data.lastName;
                form.querySelector('[name="Email"]').value = data.email;
                form.querySelector('[name="PhoneNumber"]').value = data.phoneNumber ?? '';
                form.querySelector('[name="JobTitle"]').value = data.jobTitle ?? '';
                form.querySelector('[name="StreetName"]').value = data.address?.streetName ?? '';
                form.querySelector('[name="PostalCode"]').value = data.address?.postalCode ?? '';
                form.querySelector('[name="City"]').value = data.address?.city ?? '';
                form.querySelector('[name="DateOfBirth"]').value = data.dateOfBirth?.split("T")[0] ?? '';
                form.querySelector('[name="SelectedRole"]').value = data.selectedRole ?? '';

                //show existing image
                const imagePreview = form.querySelector('.image-preview');
                if (imagePreview && data.imagePath) {
                    imagePreview.src = `/${data.imagePath}`;
                    imagePreview.closest('.image-previewer')?.classList.add('selected');
                }

            } catch (error) {
                console.error('Error collecting member data:', error);
            }
        });
    });
}

//6. Pre-filling the form when editing client. Generated by chatGPT4o
function initializeClientModals() {
    document.querySelectorAll('[data-client-id]').forEach(button => {
        button.addEventListener('click', async () => {
            const clientId = button.getAttribute('data-client-id');
            const modalId = button.getAttribute('data-target');
            const modal = document.querySelector(modalId);
            const form = modal.querySelector('form');

            try {
                const response = await fetch(`/Clients/GetClient?id=${clientId}`);
                if (!response.ok) throw new Error('Client could not be fetched');

                const data = await response.json();


                form.querySelector('[name="Id"]').value = data.id;
                form.querySelector('[name="ClientName"]').value = data.clientName;
                form.querySelector('[name="Email"]').value = data.email;
                form.querySelector('[name="Location"]').value = data.location ?? '';
                form.querySelector('[name="Phone"]').value = data.phone ?? '';


                const imagePreview = form.querySelector('.image-preview');
                if (imagePreview && data.imagePath) {
                    imagePreview.src = `/${data.imagePath}`;
                    imagePreview.closest('.image-previewer')?.classList.add('selected');
                }

            } catch (error) {
                console.error('Error collecting client data:', error);
            }
        });
    });
}

//7. Handle image previewer
function initializeImagePreview(previewSize) {
    document.querySelectorAll('.image-previewer').forEach(previewer => {
        const fileInput = previewer.querySelector('input[type="file"]');
        const imagePreview = previewer.querySelector('.image-preview');

        previewer.addEventListener('click', () => fileInput.click());

        fileInput.addEventListener('change', ({ target: { files } }) => {
            const file = files[0];
            if (file) {
                processImage(file, imagePreview, previewer, previewSize);
            }
        });
    });
}

//8. Pre-filling the form when editing project. 
function initializeProjectModals() {
    document.querySelectorAll('[data-project-id]').forEach(button => {
        button.addEventListener('click', async () => {
            const projectId = button.getAttribute('data-project-id');
            const modalId = button.getAttribute('data-target');
            const modal = document.querySelector(modalId);
            const form = modal.querySelector('form');

            try {
                const response = await fetch(`/Projects/GetProject?id=${projectId}`);

                if (!response.ok)
                    throw new Error('Project could not be fetched');

                const data = await response.json();

                //pre-filling form
                if (data) {
                    form.querySelector('[name="Id"]').value = data.id;
                    form.querySelector('[name="ProjectName"]').value = data.projectName;
                    form.querySelector('[name="ClientId"]').value = data.clientId;
                    form.querySelector('[name="Description"]').value = data.description ?? '';
                    form.querySelector('[name="StartDate"]').value = data.startDate?.split("T")[0] ?? '';
                    form.querySelector('[name="EndDate"]').value = data.endDate?.split("T")[0] ?? '';

                    const membersSelect = form.querySelector('[name="MemberIds"]');
                    if (membersSelect && data.memberIds?.length) {
                        const ts = membersSelect.tomselect;
                        ts?.setValue(data.memberIds.map(id => id.toString()));
                    }

                    form.querySelector('[name="Budget"]').value = data.budget ?? '';
                    form.querySelector('[name="StatusId"]').value = data.statusId;

                    //show existing image
                    const imagePreview = form.querySelector('.image-preview');
                    if (imagePreview && data.imagePath) {
                        imagePreview.src = `/${data.imagePath}`;
                        imagePreview.closest('.image-previewer')?.classList.add('selected');
                    }

                    // Update quill-editor with description
                    const quillEditor = modal.querySelector('.ql-editor');
                    if (quillEditor) {
                        quillEditor.innerHTML = data.description ?? '';
                    }
                }
            } catch (error) {
                console.error('Error collecting project data:', error);
            }
        });
    });
}

//9. Close modal
function initializeCloseModals() {
    document.querySelectorAll('[data-close="true"]').forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal');
            if (modal) {

                // Clear form
                modal.querySelectorAll('form').forEach(form => {
                    form.reset();
                    clearErrorMessages(form);

                    // Clear image preview
                    const imagePreview = form.querySelector('.image-preview');
                    if (imagePreview) {
                        imagePreview.src = '';
                    }
                    const imagePreviewer = form.querySelector('.image-previewer');
                    if (imagePreviewer) {
                        imagePreviewer.classList.remove('selected');
                    }
                });
                modal.style.display = 'none';
            }
        });
    });
}

//10. Dark mode
function initializeDarkMode() {
    const darkmodeSwitch = document.querySelector('#darkmode-switch');
    const root = document.documentElement;

    if (!darkmodeSwitch) return; 

    const hasDarkmode = localStorage.getItem('darkmode');

    if (hasDarkmode == null) {
        if (window.matchMedia('(prefers-color-scheme: dark)').matches) {
            enableDarkMode();
        } else {
            disableDarkMode();
        }
    } else if (hasDarkmode === 'on') {
        enableDarkMode();
    } else if (hasDarkmode === 'off') {
        disableDarkMode();
    }

    darkmodeSwitch.addEventListener('change', () => {
        if (darkmodeSwitch.checked) {
            enableDarkMode();
            localStorage.setItem('darkmode', 'on');
        } else {
            disableDarkMode();
            localStorage.setItem('darkmode', 'off');
        }
    });

    function enableDarkMode() {
        darkmodeSwitch.checked = true;
        root.classList.add('dark');
    }

    function disableDarkMode() {
        darkmodeSwitch.checked = false;
        root.classList.remove('dark');
    }
}



// clear error messages
function clearErrorMessages(form) {
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error');
    });

    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = '';
        span.classList.remove('field-validation-error');
    });
}


// load image
async function loadImage(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();

        reader.onerror = () => reject(new Error('Error reading file'));
        reader.onload = (e) => {
            const img = new Image();
            img.onerror = () => reject(new Error('Error loading image'));
            img.onload = () => resolve(img);
            img.src = e.target.result;
        };

        reader.readAsDataURL(file);
    });
}

// handle image preview
async function processImage(file, imagePreview, previewer, previewSize = 150) {
    try {
        const img = await loadImage(file);
        const canvas = document.createElement('canvas');
        canvas.width = previewSize;
        canvas.height = previewSize;

        const ctx = canvas.getContext('2d');
        ctx.drawImage(img, 0, 0, previewSize, previewSize);
        imagePreview.src = canvas.toDataURL('image/jpeg');
        previewer.classList.add('selected');
    } catch (error) {
        console.error('Failed on image-processing', error);
    }
}

//Quill wysiwyg
function initWysiwyg(wysiwygEditorId, wysiwygToolbarId, textareaId, content) {
    const textarea = document.querySelector(textareaId);
    const quill = new Quill(wysiwygEditorId, {
        modules: {
            syntax: true,
            toolbar: wysiwygToolbarId
        },
        placeholder: 'Type something',
        theme: 'snow'
    });

    if (content) quill.root.innerHTML = content;

    quill.on('text-change', () => {
        textarea.value = quill.root.innerHTML;
    });
}








