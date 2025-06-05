document.addEventListener("DOMContentLoaded", () => {
    const genres = window.topGenres;
    const container = document.getElementById('bubbleContainer');
    if (!container || !genres) return;

    const width = container.clientWidth;
    const height = container.clientHeight;

    const spotifyGreen = '#1DB954';
    const darkGreys = ['#2E2E2E', '#3C3C3C', '#4A4A4A', '#5A5A5A', '#6B6B6B'];

    genres.forEach((genre, i) => {
        const bubble = document.createElement('div');
        bubble.textContent = genre;
        bubble.style.position = 'absolute';
        bubble.style.padding = '12px 18px';
        bubble.style.borderRadius = '50%';
        bubble.style.color = '#fff';
        bubble.style.fontWeight = '600';
        bubble.style.userSelect = 'none';
        bubble.style.cursor = 'default';
        bubble.style.boxShadow = '0 4px 12px rgba(0,0,0,0.7)';
        bubble.style.display = 'flex';
        bubble.style.justifyContent = 'center';
        bubble.style.alignItems = 'center';
        bubble.style.textAlign = 'center';
        bubble.style.width = '80px';
        bubble.style.height = '80px';

        // Pick color: spotify green for first bubble, then dark greys cycling
        if (i === 0) {
            bubble.style.background = spotifyGreen;
        } else {
            bubble.style.background = darkGreys[(i - 1) % darkGreys.length];
        }

        // Random start position inside container
        const padding = 20;
        const x = Math.random() * (width - 80 - padding * 2) + padding;
        const y = Math.random() * (height - 80 - padding * 2) + padding;

        bubble.style.left = `${x}px`;
        bubble.style.top = `${y}px`;

        // Add floating animation
        const animDuration = 4000 + Math.random() * 3000; // 4-7 seconds

        bubble.animate(
            [
                { transform: 'translateY(0)' },
                { transform: 'translateY(-15px)' },
                { transform: 'translateY(0)' }
            ],
            {
                duration: animDuration,
                iterations: Infinity,
                easing: 'ease-in-out',
                delay: i * 200 // stagger start times
            }
        );

        container.appendChild(bubble);
    });
});
