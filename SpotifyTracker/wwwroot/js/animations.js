const observer = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
        console.log(entry);
        if (entry.isIntersecting) {
            if (entry.target.classList.contains('hidden-left')) {
                entry.target.classList.add('show-left');
            }
            if (entry.target.classList.contains('hidden-right')) {
                entry.target.classList.add('show-right');
            }
            if (entry.target.classList.contains('hidden-bottom')) {
                entry.target.classList.add('show-bottom');
            }
        } else {
            if (entry.target.classList.contains('hidden-left')) {
                entry.target.classList.remove('show-left');
            }
            if (entry.target.classList.contains('hidden-right')) {
                entry.target.classList.remove('show-right');
            }
            if (entry.target.classList.contains('hidden-bottom')) {
                entry.target.classList.remove('show-bottom');
            }
        }
    });
});

const hiddenElementsLeft = document.querySelectorAll('.hidden-left');
const hiddenElementsRight = document.querySelectorAll('.hidden-right');
const hiddenElementsBottom = document.querySelectorAll('.hidden-bottom');

hiddenElementsLeft.forEach((el) => observer.observe(el));
hiddenElementsRight.forEach((el) => observer.observe(el));
hiddenElementsBottom.forEach((el) => observer.observe(el));
