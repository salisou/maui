package com.microsoft.maui;

import android.view.View;
import android.view.ViewGroup;
import android.view.ViewParent;

public class ViewHelper {
    private static final int AutomationId  = 1 << 0;
    private static final int Visibility    = 1 << 1;
    private static final int FlowDirection = 1 << 2;
    private static final int MinimumHeight = 1 << 3;
    private static final int MinimumWidth  = 1 << 4;
    private static final int IsEnabled     = 1 << 5;
    private static final int Opacity       = 1 << 6;
    private static final int TranslationX  = 1 << 7;
    private static final int TranslationY  = 1 << 8;
    private static final int Scale         = 1 << 9;
    private static final int ScaleX        = 1 << 10;
    private static final int ScaleY        = 1 << 11;
    private static final int Rotation      = 1 << 12;
    private static final int RotationX     = 1 << 13;
    private static final int RotationY     = 1 << 14;
    private static final int AnchorX       = 1 << 15;
    private static final int AnchorY       = 1 << 16;

    public static void requestLayoutIfNeeded(View view)
    {
        if (!view.isInLayout())
            view.requestLayout();
    }

    public static void removeFromParent(View view)
    {
        ViewParent parent = view.getParent();
        if (parent == null)
            return;
        ((ViewGroup)parent).removeView(view);
    }

    public static void setPivotXIfNeeded(View view, float pivotX)
    {
        if (view.getPivotX() != pivotX)
            view.setPivotX(pivotX);
    }

    public static void setPivotYIfNeeded(View view, float pivotY)
    {
        if (view.getPivotY() != pivotY)
            view.setPivotY(pivotY);
    }

    public static void set(
        View view,
        int propertyMask,
        int automationTagId,
        String automationId,
        int visibility,
        int layoutDirection,
        int minimumHeight,
        int minimumWidth,
        boolean enabled,
        float alpha,
        float translationX,
        float translationY,
        float scaleX,
        float scaleY,
        float rotation,
        float rotationX,
        float rotationY,
        float pivotX,
        float pivotY)
    {
        requestLayoutIfNeeded(view);
        if ((propertyMask & AutomationId) != 0)
            view.setTag(automationTagId, automationId);
        if ((propertyMask & Visibility) != 0)
            view.setVisibility(visibility);
        if ((propertyMask & FlowDirection) != 0)
            view.setLayoutDirection(layoutDirection);
        if ((propertyMask & MinimumHeight) != 0)
            view.setMinimumHeight(minimumHeight);
        if ((propertyMask & MinimumWidth) != 0)
            view.setMinimumWidth(minimumWidth);
        if ((propertyMask & IsEnabled) != 0)
            view.setEnabled(enabled);
        if ((propertyMask & Opacity) != 0)
            view.setAlpha(alpha);
        if ((propertyMask & TranslationX) != 0)
            view.setTranslationX(translationX);
        if ((propertyMask & TranslationY) != 0)
            view.setTranslationY(translationY);
        if ((propertyMask & ScaleX) != 0)
            view.setScaleX(scaleX);
        if ((propertyMask & ScaleY) != 0)
            view.setScaleY(scaleY);
        if ((propertyMask & Rotation) != 0)
            view.setRotation(rotation);
        if ((propertyMask & RotationX) != 0)
            view.setRotationX(rotationX);
        if ((propertyMask & RotationY) != 0)
            view.setRotationY(rotationY);
        if ((propertyMask & AnchorX) != 0)
            setPivotXIfNeeded(view, pivotX);
        if ((propertyMask & AnchorY) != 0)
            setPivotYIfNeeded(view, pivotY);
    }
}