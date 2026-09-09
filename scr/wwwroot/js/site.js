function showToast(message, type) {
    const wrap = document.getElementById("toastWrap");
    if (!wrap) return;
    const el = document.createElement("div");
    el.className = "toast-item" + (type === "error" ? " error" : "");
    el.textContent = message;
    wrap.appendChild(el);
    setTimeout(() => { el.style.opacity = "0"; el.style.transform = "translateX(30px)"; }, 2500);
    setTimeout(() => el.remove(), 2900);
}

function getToken() {
    return document.querySelector("input[name='__RequestVerificationToken']")?.value;
}

// AJAX add-to-cart
document.addEventListener("submit", async (e) => {
    const form = e.target;
    if (!form.classList || !form.classList.contains("js-add-cart")) return;
    e.preventDefault();
    const data = new FormData(form);
    if (!data.get("__RequestVerificationToken")) {
        data.append("__RequestVerificationToken", getToken() || "");
    }
    try {
        const res = await fetch(form.action, {
            method: "POST",
            headers: { "X-Requested-With": "XMLHttpRequest" },
            body: data
        });
        const json = await res.json();
        showToast(json.message, json.success ? "success" : "error");
        if (json.success) {
            const badge = document.getElementById("cartCount");
            if (badge) {
                badge.textContent = json.cartCount;
                badge.animate([{ transform: "scale(1)" }, { transform: "scale(1.4)" }, { transform: "scale(1)" }], { duration: 400 });
            }
        }
    } catch (err) {
        showToast("Không thể thêm vào giỏ hàng", "error");
    }
});

// gallery thumbs
document.querySelectorAll(".thumb").forEach((btn) => {
    btn.addEventListener("click", () => {
        const main = document.getElementById("mainImage");
        if (!main) return;
        main.src = btn.dataset.src;
        document.querySelectorAll(".thumb").forEach(t => t.classList.remove("active"));
        btn.classList.add("active");
    });
});

// header scroll effect
const header = document.getElementById("siteHeader");
const scrollTopBtn = document.getElementById("scrollTop");
window.addEventListener("scroll", () => {
    if (window.scrollY > 20) header?.classList.add("scrolled"); else header?.classList.remove("scrolled");
    if (window.scrollY > 400) scrollTopBtn?.classList.add("visible"); else scrollTopBtn?.classList.remove("visible");
});
scrollTopBtn?.addEventListener("click", () => window.scrollTo({ top: 0, behavior: "smooth" }));

// mobile mega menu toggle
document.querySelectorAll(".has-menu > a").forEach(link => {
    link.addEventListener("click", (e) => {
        if (window.innerWidth <= 991) {
            e.preventDefault();
            link.parentElement.classList.toggle("open");
        }
    });
});

// reveal on scroll
const io = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.classList.add("in");
            io.unobserve(entry.target);
        }
    });
}, { threshold: 0.12 });
document.querySelectorAll(".reveal").forEach(el => io.observe(el));

// image fallback for broken remote images
document.addEventListener("error", (e) => {
    const t = e.target;
    if (t.tagName === "IMG" && !t.dataset.fallback) {
        t.dataset.fallback = "1";
        t.src = "/images/products/placeholder.svg";
    }
}, true);
