let currentIndex = 0;
let newsList = [];

async function fetchHockeyNews() {
    try {
        const response = await fetch('/nhl/news');
        if (!response.ok) {
            throw new Error('Ошибка получении новостей: ' + response.statusText);
        }

        newsList = await response.json();

        if (newsList.length === 0) {
            displayNoNews();
        } else {
            displayNews(currentIndex);
        }
    } catch (error) {
        console.error(error);
    }
}

function displayNoNews() {
    const newsContainer = document.getElementById('news');
    newsContainer.innerHTML = '<p>нету новостей.</p>';
}

function displayNews(index) {
    const newsContainer = document.getElementById('news');
    newsContainer.innerHTML = '';

    const item = newsList[index];
    const newsItem = document.createElement('article');
    newsItem.className = 'news-item';

    newsItem.innerHTML =
        `<div class="news-content">
            <p style="font-size: larger; font-weight: bold;">${item.headline}</p>
            <p>${item.description}</p>
            <a href="${item.link}" style="color: white;" target="_blank">More details</a>
            <p style="font-size: small;">Date: ${formatDate(item.published)}</p>
        </div>`;

    newsContainer.appendChild(newsItem);
}

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('ru-RU', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit',
        hour12: false
    });
}

document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('fetch-news-button').addEventListener('click', () => {
        currentIndex = (currentIndex + 1) % newsList.length;
        displayNews(currentIndex);
    });

    fetchHockeyNews();
});
