import re


def get_months(s):
    """
    >>> get_months("Dec18-Jan20 (14mth)-Last")
    14
    >>> get_months("Dec17-Nov19 (445 months)")
    445
    >>> get_months("for 3 Events (1st Event - Oct19)")
    >>> get_months("Apr19-Dec19 (9 mths)")
    9
    """
    match = re.search(r"(\d+)\s*(mth|months)", s)
    if not match:
        return None
    return int(match.group(1))


if __name__ == "__main__":
    import doctest

    doctest.testmod()

