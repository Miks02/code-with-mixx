

export function initHeroAnimation() {
    VANTA.FOG({
        el: "#vanta-bg",
        mouseControls: true,
        touchControls: true,
        gyroControls: false,
        minHeight: 200.00,
        minWidth: 200.00,
        highlightColor: 0x10b981,
        midtoneColor: 0x065f46,
        lowlightColor: 0x064e3b,
        baseColor: 0x064e3b,
        blurFactor: 0.20,
        speed: 0.50,
        zoom: 1.00
    });
}