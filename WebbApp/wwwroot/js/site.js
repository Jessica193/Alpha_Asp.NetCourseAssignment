document.addEventListener('DOMContentLoaded', () => {
    const previewSize = 150;

    // Open modal
    document.querySelectorAll('[data-modal="true"]').forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target');
            const modal = document.querySelector(modalTarget);
            if (modal) {
                modal.style.display = 'flex';
            }
        });
    });

    // Close modal
    document.querySelectorAll('[data-close="true"]').forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal');
            if (modal) {
                modal.style.display = 'none';

                // Clear form
                modal.querySelectorAll('form').forEach(form => {
                    form.reset();

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
            }
        });
    });

    // Handle image previewer
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

    // Handle form submission
//    const forms = document.querySelectorAll('form');
//    forms.forEach(form => {
//        form.addEventListener('submit', async (e) => {
//            e.preventDefault();
         
//            clearErrorMessages(form);
          
//            const formData = new FormData(form);
          
//            try {
//                const res = await fetch(form.action, {
//                    method: 'POST',
//                    body: formData
//                });

//                console.log(res);  //TA BORT SEN

//                if (res.ok) {
//                    const modal = form.closest('.modal');
//                    if (modal) 
//                        modal.style.display = 'none';

//                        window.location.reload();  
//                }
//                else if (res.status === 400) {
//                    const data = await res.json();

//                    console.log(data);  //TA BORT SEN

//                    if (data.errors) {
//                        Object.keys(data.errors).forEach(key => {

//                            console.log(1);  //TA BORT SEN

//                            let input = form.querySelector(`[name="${key}"]`);
//                            if (input) {
//                                input.classList.add('input-validation-error');
//                            }

//                            const span = form.querySelector(`[data-valmsg-for="${key}"]`);
//                            if (span) {
//                                span.innerText = data.errors[key].join('\n');
//                                span.classList.add('field-validation-error');
//                            }
//                        });
//                    }
//                }
//            }
//            catch (e) {
//                console.error('Error submitting the form', e);
//            }
//        });
//    });
//});

// Funktion för att rensa felmeddelanden
function clearErrorMessages(form) {
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error');
    });

    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = '';
        span.classList.remove('field-validation-error');
    });
}


// Funktion för att ladda in bild
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

// Funktion för att hantera bildförhandsvisning
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
